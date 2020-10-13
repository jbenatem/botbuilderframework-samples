// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using SaveDataInJsonBot.Model;

namespace SaveDataInJsonBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        HistorialMensajes historialMensajes = new HistorialMensajes();
        public List<MensajeEnviado> ListaMensajes = new List<MensajeEnviado>();
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Generar objeto MensajeUsuario con los datos del último mensaje enviado por el usuario
            MensajeEnviado MensajeUsuario = await CrearObjetoMensajeEnviado(turnContext);
            //Guardar objeto MensajeUsuario en el historial de mensajes        
            ListaMensajes.Add(MensajeUsuario);
            historialMensajes.MensajesEnviados = ListaMensajes;
            //Guardamos el objeto data en un archivo JSON
            GuardarMensajeEnJson(historialMensajes);
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        private async Task<MensajeEnviado> CrearObjetoMensajeEnviado(ITurnContext turnContext)
        {
            //Definimos la clase MensajeEnviado como objeto
            MensajeEnviado data = new MensajeEnviado();
            //Obtenemos valores del context y guardamos valores en el objeto data
            data.Emisor = turnContext.Activity.From.Name;
            data.Receptor = turnContext.Activity.Recipient.Name;
            data.Mensaje = turnContext.Activity.Text;
            data.Timestamp = turnContext.Activity.Timestamp.ToString();
            return data;
        }
        private void GuardarMensajeEnJson(HistorialMensajes HistorialMensajes)
        {
           //Serializamos el objeto data
           string JSONresult = JsonConvert.SerializeObject(HistorialMensajes);
            //Definimos la ruta del archivo JSON
            string path = "HistorialMensajes.json";
            //Si el archivo existe, se elimina y se crea uno nuevo
            if (File.Exists(path))
           {
                File.Delete(path);
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }    
           }
            //Si el archivo no existe se crea directamente uno nuevo
            else if (!File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }
            }
        }
    }
}

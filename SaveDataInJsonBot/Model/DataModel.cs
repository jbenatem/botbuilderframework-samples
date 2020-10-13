using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveDataInJsonBot.Model
{
    public class MensajeEnviado
    {
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string Mensaje { get; set; }
        public string Timestamp { get; set; }
    }
    public class HistorialMensajes
    {
        public List<MensajeEnviado> MensajesEnviados { get; set; }
    }
}

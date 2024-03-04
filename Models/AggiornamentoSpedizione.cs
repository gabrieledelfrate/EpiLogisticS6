using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiLogistic.Models
{
    public class AggiornamentoSpedizione
    {
        public int Id { get; set; }

        [Required]
        public int SpedizioneId { get; set; }

        [Required]
        public string Stato { get; set; }

        public string Luogo { get; set; }

        public string Descrizione { get; set; }

        [Required]
        public DateTime DataOraAggiornamento { get; set; }
    }

}
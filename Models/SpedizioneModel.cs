using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiLogistic.Models
{
    public class SpedizioneModel
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int NumeroIdentificativo { get; set; }

        [Required]
        public DateTime DataSpedizione { get; set; }

        [Required]
        public decimal Peso { get; set; }

        [Required]
        public string CittaDestinataria { get; set; }

        [Required]
        public string IndirizzoDestinatario { get; set; }

        [Required]
        public string NominativoDestinatario { get; set; }

        [Required]
        public decimal Costo { get; set; }

        [Required]
        public DateTime DataConsegnaPrevista { get; set; }
    }

}
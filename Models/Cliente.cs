using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiLogistic.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public bool IsAzienda { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il Codice Fiscale è obbligatorio.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il Codice Fiscale deve essere di 16 caratteri.")]
        public string CodiceFiscale { get; set; }

        [Required(ErrorMessage = "La Partita IVA è obbligatoria per le aziende.")]
        public string PartitaIva { get; set; }

        public string Citta { get; set; }

        public string Indirizzo { get; set; }

        public string CAP { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }
    }
}
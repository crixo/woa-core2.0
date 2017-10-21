using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Woa.Models
{
    public class Paziente
    {
        public int ID { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Indirizzo { get; set; }

        public string Citta { get; set; }

        public string Professione { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Cellulare { get; set; }

        public string Prov { get; set; }

        [MaxLength(5)]
        public string CAP { get; set; }

        [Column("data_nascita")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataDiNascita { get; set; }

        public Paziente()
        {
        }
    }
}

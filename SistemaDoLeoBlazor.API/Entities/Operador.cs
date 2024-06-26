﻿using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Operador
    {
        public int id { get; set; }

        [MaxLength(50)]
        public required string nome { get; set; } = string.Empty;

        [MaxLength(20)]
        public required string senha { get; set; } = string.Empty;
        public bool admin {  get; set; }
        public bool inativo { get; set; }


        public ICollection<OperadorTela> operadorTelas = new List<OperadorTela>();
    }
}

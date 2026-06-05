using System.ComponentModel.DataAnnotations;

namespace MySpend.Models.Entities
//Es como una "carpeta virtual". Sirve para organizar tu código y evitar que,
//si tienes otra clase llamada Expense en otra parte del proyecto,
//el programa se confunda. Indica que esta clase pertenece al grupo de modelos de tu proyecto.
{
    //Es una clase para los gastos
    public class Expense
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;


        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;


       

        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.UtcNow;

    }
}

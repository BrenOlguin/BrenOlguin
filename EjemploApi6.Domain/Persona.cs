using System.ComponentModel.DataAnnotations;

namespace EjemploApi6.Domain;

public  class Persona
{
    [Key]
    public int Id { get; set; }
    public int Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int Edad { get; set; }
    public string Nacionalidad { get; set; }
}

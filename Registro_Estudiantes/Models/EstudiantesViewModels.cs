using System; // Agregado para DateTime
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para Data Annotations
using Microsoft.AspNetCore.Mvc.Rendering; // Para SelectListItem

// ELIMINAR ESTAS LÍNEAS. ESTÁN CAUSANDO EL ERROR DE HTTPCONTEXT.
// using System.Linq; // No es estrictamente necesario en el ViewModel, aunque no causa conflicto.
// using Microsoft.AspNetCore.Mvc; // ESTO ES LO QUE CAUSABA EL ERROR "HttpContext". ¡ELIMINAR!
// using Microsoft.AspNetCore.Routing.Constraints; // No necesario en el ViewModel. ¡ELIMINAR!

namespace Registro_Estudiantes.Models
{
    public class EstudiantesViewModels
    {
        // Constructor para inicializar las listas desplegables.
        // Se recomienda inicializarlas aquí para evitar NullReferenceException
        // si el modelo se crea sin ser un POST back.
        public EstudiantesViewModels()
        {
            opcionescarrera = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = " selecciona una carrera "},
                new SelectListItem { Value = "IngenieriaSoftware", Text = "Ingenieria de Software"},
                new SelectListItem { Value = "Derecho", Text = "Derecho"},
                new SelectListItem { Value = "Medicina", Text = "Medicina"},
                new SelectListItem { Value = "Arquitectura", Text = "Arquitectura"}
            };

            Opcionesturno = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = " selecciona un turno "},
                new SelectListItem { Value = "manana", Text = " mañana "},
                new SelectListItem { Value = "tarde", Text = " tarde "},
                new SelectListItem { Value = "noche", Text = " noche "},
            };

            Opcionestipoingreso = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = " selecciona un tipo de ingreso "},
                new SelectListItem { Value = "nuevo", Text = " nuevo ingreso "},
                new SelectListItem { Value = "transferencia", Text = " transferencia "},
                new SelectListItem { Value = "reingreso", Text = " reingreso "},
            };
        }


        [Required(ErrorMessage = "la matricula es obligatoria bro")
            , StringLength(15, MinimumLength = 5, ErrorMessage = "la matricula tiene que tener 8 numero, animal"),
            Display(Name = "Matricula")]
        // CAMBIO CRÍTICO: La propiedad debe tener la primera letra en mayúscula por convención de C# (PascalCase)
        // y para que el model binding y los Tag Helpers de ASP.NET Core funcionen correctamente y de forma consistente.
        public string Matricula { get; set; } // Propiedad corregida a PascalCase

        [Required(ErrorMessage = " tu nombre es obligatorio, yegua"),
            Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "compay el correo electronico es obligatorio."),
            EmailAddress(ErrorMessage = "disk no sabe tu que correo electronico no es correcto"),
            Display(Name = "correo electronico ")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "wea klk la fecha de nacimento eh obligatoria."),
            DataType(DataType.Date, ErrorMessage = "ehh bro asi no se escribe la fecha."),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), // Agregado {0: para formato}
            Display(Name = "fecha de nacimiento")]
        public DateTime Edad { get; set; } // Propiedad 'Edad' (tipo DateTime), aunque 'FechaNacimiento' es más descriptivo

        [Required(ErrorMessage = "la carrera es obligatorio."), Display(Name = "Carrera")]
        public string carrera { get; set; } // Propiedad 'carrera' (minúscula), mantener para no cambiar vistas a menos que sea necesario.
        public List<SelectListItem> opcionescarrera { get; set; } // Propiedad 'opcionescarrera' (minúscula)


        [Required(ErrorMessage = "por favor, el telefono. ")
            , Phone(ErrorMessage = "repito disk tu no sabe que telefono no es correcto. "),
            StringLength(10, MinimumLength = 10, ErrorMessage = " wey y klk son 10 numero, no 5 animal"),
            Display(Name = "telefono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = " yo no tengo problema con los pajaron, pero elige oka."),
            Display(Name = "genero")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Mio toma un turno por favor."),
            Display(Name = "turno")]
        public string Turno { get; set; }
        public List<SelectListItem> Opcionesturno { get; set; } // Propiedad 'Opcionesturno' (PascalCase)

        [Required(ErrorMessage = "Ven aca palomo tu eres nuevo si o no?"),
            Display(Name = "tipo de ingreso")]
        public string TipoIngreso { get; set; }
        public List<SelectListItem> Opcionestipoingreso { get; set; } // Propiedad 'Opcionestipoingreso' (PascalCase)

        [Display(Name = "y tu eres becado o poppy?")]
        // CAMBIO CRÍTICO: La propiedad debe tener la primera letra en mayúscula por convención de C# (PascalCase)
        // y para que el model binding y los Tag Helpers de ASP.NET Core funcionen correctamente y de forma consistente.
        public bool EstaBecado { get; set; } // Propiedad corregida a PascalCase

        [Range(0, 100, ErrorMessage = "primero lo primero el porcentaje tiene que esta medio alto oka."),
            Display(Name = "porcentaje de beca")]
        public int? PorcentajeBeca { get; set; }

        [Required(ErrorMessage = "barbaro te sacaste la loteria firma ahi al favor para darte ese dinero y sali de eso tu sabe."),
            Range(typeof(bool), "true", "true", ErrorMessage = "Mio pero y klk firma o que fue"),
            Display(Name = " acepta lo termino y condiciones ")]
        // CAMBIO CRÍTICO: La propiedad debe tener la primera letra en mayúscula por convención de C# (PascalCase)
        // y para que el model binding y los Tag Helpers de ASP.NET Core funcionen correctamente y de forma consistente.
        public bool TerminosYCondiciones { get; set; } // Propiedad corregida a PascalCase
    }
}
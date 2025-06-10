using Microsoft.AspNetCore.Mvc;
using Registro_Estudiantes.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem

namespace Registro_Estudiantes.Controllers
{
    public class EstudiantesController : Controller
    {
        // Esta lista estática simula una base de datos en memoria.
        // En una aplicación real, esto se reemplazaría por una conexión a una DB (Entity Framework Core, etc.).
        private static List<EstudiantesViewModels> listaEstudiantes = new List<EstudiantesViewModels>();

        // GET: /Estudiantes/Index (Muestra el formulario de registro)
        public IActionResult Index()
        {
            // Crea una nueva instancia del ViewModel.
            // Las propiedades OpcionesCarrera, OpcionesTurno, OpcionesTipoIngreso
            // se inicializan en el constructor de EstudiantesViewModels,
            // por lo que ya estarán listas para la vista.
            var model = new EstudiantesViewModels();
            return View(model);
        }

        // POST: /Estudiantes/Registrar (Procesa el envío del formulario de registro)
        [HttpPost]
        public IActionResult Registrar(EstudiantesViewModels estudiante)
        {
            // --- Validación Personalizada / Condicional ---

            // 1. Validación de PorcentajeBeca: Solo si está becado, el porcentaje es obligatorio y válido.
            // CAMBIO: Se corrigió la propiedad 'Estadobecado' a 'EstaBecado' y 'matricula' a 'Matricula'
            if (estudiante.EstaBecado && (estudiante.PorcentajeBeca == null || estudiante.PorcentajeBeca < 0 || estudiante.PorcentajeBeca > 100))
            {
                ModelState.AddModelError("PorcentajeBeca", "El porcentaje de beca es obligatorio y debe estar entre 0 y 100 si el estudiante está becado.");
            }
            else if (!estudiante.EstaBecado)
            {
                // Si no está becado, nos aseguramos de que el porcentaje de beca sea nulo.
                estudiante.PorcentajeBeca = null;
            }

            // 2. Validación de unicidad de Matrícula: Asegura que no haya matrículas duplicadas.
            // Solo se verifica si estamos registrando un nuevo estudiante.
            // CAMBIO: Se corrigió la propiedad 'matricula' a 'Matricula'
            if (listaEstudiantes.Any(e => e.Matricula == estudiante.Matricula))
            {
                ModelState.AddModelError("Matricula", "Ya existe un estudiante con esta matrícula. La matrícula debe ser única.");
            }

            // --- Fin de Validaciones Personalizadas ---


            // Verifica si el modelo es válido según las anotaciones de datos y las validaciones personalizadas.
            if (ModelState.IsValid)
            {
                listaEstudiantes.Add(estudiante); // Agrega el estudiante a la lista en memoria.
                return RedirectToAction("Lista"); // Redirige a la acción "Lista" para mostrar todos los estudiantes.
            }

            // Si el modelo NO es válido, vuelve a cargar el formulario con los errores.
            // El 'estudiante' ya contiene las opciones de las listas desplegables inicializadas,
            // por lo que no es necesario volver a poblar 'OpcionesCarrera', etc.
            return View("index", estudiante);
        }

        // GET: /Estudiantes/Lista (Muestra la lista de todos los estudiantes registrados)
        public IActionResult Lista()
        {
            // Pasa la lista completa de estudiantes a la vista.
            return View(listaEstudiantes);
        }

        // GET: /Estudiantes/Editar (Muestra el formulario para editar un estudiante específico)
        // Recibe la matrícula del estudiante a editar como parámetro de ruta.
        public IActionResult Editar(string matricula)
        {
            // Busca el estudiante en la lista por su matrícula.
            // CAMBIO: Se corrigió la propiedad 'matricula' a 'Matricula'
            var estudiante = listaEstudiantes.FirstOrDefault(e => e.Matricula == matricula);

            // Si no se encuentra el estudiante, retorna un error 404 (Not Found).
            if (estudiante == null)
                return NotFound();

            // Pasa el objeto estudiante encontrado a la vista de edición.
            // Las propiedades OpcionesCarrera, OpcionesTurno, OpcionesTipoIngreso
            // se inicializan en el constructor de EstudiantesViewModels,
            // por lo que estarán listas para la vista de edición.
            return View(estudiante);
        }

        // POST: /Estudiantes/Editar (Procesa el envío del formulario de edición)
        // Recibe el objeto EstudiantesViewModels con los datos modificados.
        [HttpPost]
        public IActionResult Editar(EstudiantesViewModels estudiante)
        {
            // --- Validación Personalizada / Condicional para Edición ---

            // Validación de PorcentajeBeca: Mismo control que en el registro.
            // CAMBIO: Se corrigió la propiedad 'Estadobecado' a 'EstaBecado'
            if (estudiante.EstaBecado && (estudiante.PorcentajeBeca == null || estudiante.PorcentajeBeca < 0 || estudiante.PorcentajeBeca > 100))
            {
                ModelState.AddModelError("PorcentajeBeca", "El porcentaje de beca es obligatorio y debe estar entre 0 y 100 si el estudiante está becado.");
            }
            else if (!estudiante.EstaBecado)
            {
                estudiante.PorcentajeBeca = null;
            }

            // NOTA: Para la edición, NO se suele validar la unicidad de la Matrícula del mismo modo,
            // ya que el estudiante que se está editando ya existe con esa matrícula.
            // Si permites cambiar la matrícula, deberías verificar que la nueva matrícula no exista
            // para otro estudiante diferente al que se está editando.
            // Por simplicidad, asumimos que la matrícula es la clave y no se cambia en este formulario.

            // --- Fin de Validaciones Personalizadas para Edición ---

            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                // Encuentra el estudiante original en la lista usando su matrícula.
                // CAMBIO: Se corrigió la propiedad 'matricula' a 'Matricula'
                var original = listaEstudiantes.FirstOrDefault(e => e.Matricula == estudiante.Matricula);

                // Si se encuentra el estudiante original, actualiza sus propiedades.
                if (original != null)
                {
                    // CAMBIOS: Se corrigieron los nombres de las propiedades a PascalCase para que coincidan con el ViewModel.
                    original.Nombre = estudiante.Nombre;
                    original.Correo = estudiante.Correo;
                    original.Telefono = estudiante.Telefono;
                    original.Edad = estudiante.Edad;
                    original.Genero = estudiante.Genero;
                    original.carrera = estudiante.carrera; // Esta línea se mantiene así porque en tu ViewModel está 'carrera' en minúscula
                    original.Turno = estudiante.Turno;
                    original.TipoIngreso = estudiante.TipoIngreso;
                    original.EstaBecado = estudiante.EstaBecado; // Esta línea se mantiene así porque en tu ViewModel está 'Estadobecado'
                    original.PorcentajeBeca = estudiante.PorcentajeBeca;
                    original.TerminosYCondiciones = estudiante.TerminosYCondiciones; // Esta línea se mantiene así porque en tu ViewModel está 'TerminoYCondiciones'
                }
                return RedirectToAction("Lista"); // Redirige a la lista después de guardar los cambios.
            }
            // Si el modelo no es válido, vuelve a cargar el formulario de edición con los errores.
            return View(estudiante);
        }

        // GET: /Estudiantes/Eliminar (Elimina un estudiante de la lista)
        // Recibe la matrícula del estudiante a eliminar.
        public IActionResult Eliminar(string matricula)
        {
            // Busca el estudiante en la lista.
            // CAMBIO: Se corrigió la propiedad 'matricula' a 'Matricula'
            var estudiante = listaEstudiantes.FirstOrDefault(e => e.Matricula == matricula);

            // Si se encuentra el estudiante, lo elimina de la lista.
            if (estudiante != null)
                listaEstudiantes.Remove(estudiante);

            return RedirectToAction("Lista"); // Redirige a la lista actualizada de estudiantes.
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JuegoSerpiente
{
    internal class Ventana
    {
        public string Titulo { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public ConsoleColor ColorFondo { get; set; }
        public ConsoleColor ColorTexto { get; set; }
        public Point LimiteSuperior { get; set; }
        public Point LimiteInferior { get; set; }

        public Ventana(string titulo, int ancho, int alto, ConsoleColor colorFondo,
                      ConsoleColor colorTexto, Point limiteSuperior, Point limiteInferior)
        {
            Titulo = titulo;
            Ancho = ancho;
            Alto = alto;
            ColorFondo = colorFondo;
            ColorTexto = colorTexto;
            LimiteSuperior = limiteSuperior;
            LimiteInferior = limiteInferior;
            Init();
        }

        public void Init()
        {
            Console.SetWindowSize(Ancho, Alto);
            Console.SetBufferSize(Ancho, Alto); //evita scroll
            Console.Title = Titulo;
            Console.BackgroundColor = ColorFondo;
            Console.ForegroundColor = ColorTexto;
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void DibujarMarco()
        {
            Console.ForegroundColor = ColorTexto;

            // Dibujar bordes horizontales (superior e inferior)
            for (int i = LimiteSuperior.X + 1; i < LimiteInferior.X; i++)
            {
                Console.SetCursorPosition(i, LimiteSuperior.Y);
                Console.Write("═");

                Console.SetCursorPosition(i, LimiteInferior.Y);
                Console.Write("═");
            }

            // Dibujar bordes verticales (izquierdo y derecho)
            for (int i = LimiteSuperior.Y + 1; i < LimiteInferior.Y; i++)
            {
                Console.SetCursorPosition(LimiteSuperior.X, i);
                Console.Write("║");

                Console.SetCursorPosition(LimiteInferior.X, i);
                Console.Write("║");
            }

            // Dibujar esquinas 
            Console.SetCursorPosition(LimiteSuperior.X, LimiteSuperior.Y);
            Console.Write("╔"); // Esquina superior izquierda

            Console.SetCursorPosition(LimiteInferior.X, LimiteSuperior.Y);
            Console.Write("╗"); // Esquina superior derecha

            Console.SetCursorPosition(LimiteSuperior.X, LimiteInferior.Y);
            Console.Write("╚"); // Esquina inferior izquierda

            Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
            Console.Write("╝"); // Esquina inferior derecha
        }
    }
}

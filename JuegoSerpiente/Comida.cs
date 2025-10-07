using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoSerpiente 
{
    internal class Comida
    {
        private static readonly Random _random = new Random();

        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }
        public Snake SnakeRef { get; set; }

        public Comida(Ventana ventana, Snake snake)
        {
            VentanaC = ventana;
            SnakeRef = snake;
            Color = ConsoleColor.Yellow;
            Posicion = new Point(0, 0);
        }

        public void Generar()
        {
            // Limpiar comida anterior si existía
            if (Posicion.X != 0 || Posicion.Y != 0)
            {
                Console.SetCursorPosition(Posicion.X, Posicion.Y);
                Console.Write(" ");
            }

            // Buscar una posición libre dentro de los límites, evitando la serpiente
            int minX = VentanaC.LimiteSuperior.X + 1;
            int maxX = VentanaC.LimiteInferior.X - 1;
            int minY = VentanaC.LimiteSuperior.Y + 1;
            int maxY = VentanaC.LimiteInferior.Y - 1;

            // En caso extremo, limitar intentos razonables
            for (int intentos = 0; intentos < 1000; intentos++)
            {
                int x = _random.Next(minX, maxX + 1);
                int y = _random.Next(minY, maxY + 1);
                var candidata = new Point(x, y);

                if (candidata == SnakeRef.Cabeza) continue;
                bool ocupaCuerpo = SnakeRef.Cuerpo.Any(p => p.X == candidata.X && p.Y == candidata.Y);
                if (ocupaCuerpo) continue;

                Posicion = candidata;
                Dibujar();
                return;
            }
        }

        public void Dibujar()
        {
            // Asegurar que se dibuja dentro de los límites visibles
            int minX = VentanaC.LimiteSuperior.X + 1;
            int maxX = VentanaC.LimiteInferior.X - 1;
            int minY = VentanaC.LimiteSuperior.Y + 1;
            int maxY = VentanaC.LimiteInferior.Y - 1;

            int x = Math.Max(minX, Math.Min(Posicion.X, maxX));
            int y = Math.Max(minY, Math.Min(Posicion.Y, maxY));

            Console.ForegroundColor = Color;
            Console.SetCursorPosition(x, y);
            Console.Write("@");
        }
    }
}
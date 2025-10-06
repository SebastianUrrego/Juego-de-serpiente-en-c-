using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoSerpiente
{
    internal class Snake
    {
        enum Direccion { Arriba, Abajo, Izquierda, Derecha }
        public bool Vivo { get; set; }
        public ConsoleColor ColorCabeza { get; set; }
        public ConsoleColor ColorCuerpo { get; set; }
        public Ventana VentanaC { get; set; }
        public List<Point> Cuerpo { get; set; }
        public Point Cabeza { get; set; }
        private Direccion _direccion;
        private Direccion _direccionSiguiente;
        private bool _debeCrecer;

        public Snake(Point posicion, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventana)
        {
            ColorCabeza = colorCabeza;
            ColorCuerpo = colorCuerpo;
            VentanaC = ventana;
            Cabeza = posicion;
            Cuerpo = new List<Point>();

            // Inicializar con algunos segmentos (CORREGIDO para evitar superposición)
            Cuerpo.Add(new Point(Cabeza.X - 1, Cabeza.Y));
            Cuerpo.Add(new Point(Cabeza.X - 2, Cabeza.Y));

            _direccion = Direccion.Derecha;
            _direccionSiguiente = Direccion.Derecha;
            Vivo = true;
            _debeCrecer = false;

            Dibujar();
        }

        public void CambiarDireccion(ConsoleKey tecla)
        {
            switch (tecla)
            {
                case ConsoleKey.UpArrow:
                    if (_direccion != Direccion.Abajo)
                        _direccionSiguiente = Direccion.Arriba;
                    break;
                case ConsoleKey.DownArrow:
                    if (_direccion != Direccion.Arriba)
                        _direccionSiguiente = Direccion.Abajo;
                    break;
                case ConsoleKey.LeftArrow:
                    if (_direccion != Direccion.Derecha)
                        _direccionSiguiente = Direccion.Izquierda;
                    break;
                case ConsoleKey.RightArrow:
                    if (_direccion != Direccion.Izquierda)
                        _direccionSiguiente = Direccion.Derecha;
                    break;
            }
        }

        public void Mover()
        {
            if (!Vivo) return;

            _direccion = _direccionSiguiente;

            // Guardar la posición anterior de la cabeza
            Point cabezaAnterior = Cabeza;

            MoverCabeza();

            // Mover el cuerpo
            if (Cuerpo.Count > 0)
            {
                MoverCuerpo(cabezaAnterior);
            }

            // Verificar colisiones
            VerificarColisiones();
        }

        private void MoverCabeza()
        {
            // Limpiar posición anterior de la cabeza
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write(" ");

            // Calcular nueva posición
            switch (_direccion)
            {
                case Direccion.Derecha:
                    Cabeza = new Point(Cabeza.X + 1, Cabeza.Y);
                    break;
                case Direccion.Izquierda:
                    Cabeza = new Point(Cabeza.X - 1, Cabeza.Y);
                    break;
                case Direccion.Arriba:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y - 1);
                    break;
                case Direccion.Abajo:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y + 1);
                    break;
            }

            // Teletransporte al otro lado si sale del marco
            if (Cabeza.X <= VentanaC.LimiteSuperior.X)
                Cabeza = new Point(VentanaC.LimiteInferior.X - 1, Cabeza.Y);
            else if (Cabeza.X >= VentanaC.LimiteInferior.X)
                Cabeza = new Point(VentanaC.LimiteSuperior.X + 1, Cabeza.Y);
            else if (Cabeza.Y <= VentanaC.LimiteSuperior.Y)
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteInferior.Y - 1);
            else if (Cabeza.Y >= VentanaC.LimiteInferior.Y)
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteSuperior.Y + 1);

            // Dibujar nueva cabeza
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write("█");
        }

        private void MoverCuerpo(Point nuevaPosicion)
        {
            if (_debeCrecer)
            {
                // Agregar nuevo segmento al principio
                Cuerpo.Insert(0, nuevaPosicion);
                _debeCrecer = false;
            }
            else
            {
                // Limpiar la última posición del cuerpo
                if (Cuerpo.Count > 0)
                {
                    Point cola = Cuerpo[Cuerpo.Count - 1];
                    Console.SetCursorPosition(cola.X, cola.Y);
                    Console.Write(" ");
                }

                // Mover el cuerpo
                for (int i = Cuerpo.Count - 1; i > 0; i--)
                {
                    Cuerpo[i] = Cuerpo[i - 1];
                }

                // La primera parte del cuerpo va a donde estaba la cabeza
                if (Cuerpo.Count > 0)
                {
                    Cuerpo[0] = nuevaPosicion;
                }
            }

            // Dibujar el cuerpo
            DibujarCuerpo();
        }

        public void Crear()
        {
            _debeCrecer = true;
        }

        private void DibujarCuerpo()
        {
            Console.ForegroundColor = ColorCuerpo;
            foreach (Point segmento in Cuerpo)
            {
                Console.SetCursorPosition(segmento.X, segmento.Y);
                Console.Write("■");
            }
        }

        private void Dibujar()
        {
            // Dibujar cabeza
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write("█");

            // Dibujar cuerpo
            DibujarCuerpo();
        }

        private void VerificarColisiones()
        {
            // Colisión consigo misma
            foreach (Point segmento in Cuerpo)
            {
                if (Cabeza == segmento)
                {
                    Vivo = false;
                    break;
                }
            }
        }
    }
}
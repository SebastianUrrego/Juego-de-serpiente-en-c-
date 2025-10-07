using JuegoSerpiente;
using System.Drawing;

Ventana ventana;
Snake snake;
Comida comida;
bool jugar = true;
bool enMenu = true;
int puntuacion = 0;

void Iniciar()
{
    ventana = new Ventana("Serpiente goty", 65, 20, ConsoleColor.Black, ConsoleColor.Red,
        new Point(5, 3), new Point(59, 18));
    MostrarMenu();
}

void MostrarMenu()
{
    while (enMenu)
    {
        Console.Clear();
        ventana.DibujarMarco();

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(20, 8);
        Console.Write(" S E R P I E N T E  G O T Y ");

        Console.SetCursorPosition(20, 10);
        Console.Write("Presiona ENTER para jugar");

        Console.SetCursorPosition(20, 11);
        Console.Write("Presiona ESC para salir");

        Console.SetCursorPosition(20, 13);
        Console.Write("Controles: Flechas para mover");

        var tecla = Console.ReadKey(true).Key;
        if (tecla == ConsoleKey.Enter)
        {
            enMenu = false;
            IniciarJuego();
        }
        else if (tecla == ConsoleKey.Escape)
        {
            Environment.Exit(0);
        }
    }
}

void IniciarJuego()
{
    Console.Clear();
    ventana.DibujarMarco();
    snake = new Snake(new Point(10, 10), ConsoleColor.Yellow, ConsoleColor.Green, ventana);
    comida = new Comida(ventana, snake);
    comida.Generar();
    puntuacion = 0;
    ActualizarInfo();
    Game();
}

void Game()
{
    while (jugar && snake.Vivo)
    {

        if (Console.KeyAvailable)
        {
            var tecla = Console.ReadKey(true).Key;
            if (tecla == ConsoleKey.Escape)
            {
                enMenu = true;
                MostrarMenu();
                return;
            }
            snake.CambiarDireccion(tecla);
        }

        snake.Mover();

        // Redibujar comida por si fue borrada por la cola
        comida.Dibujar();

        // Verificar colisión con comida 
        if (snake.Cabeza.X == comida.Posicion.X && snake.Cabeza.Y == comida.Posicion.Y)
        {
            puntuacion += 10;
            snake.Crear();
            comida.Generar();
            ActualizarInfo();
        }

        System.Threading.Thread.Sleep(100);
    }

    if (!snake.Vivo)
    {
        AnimacionMuerte();
        Console.ReadKey();
        enMenu = true;
        MostrarMenu();
    }
}

void ActualizarInfo()
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.SetCursorPosition(7, 1);
    Console.Write($"Puntuación: {puntuacion}   ");
    Console.SetCursorPosition(30, 1);
    Console.Write($"Longitud: {snake.Cuerpo.Count + 1}   ");
}

void AnimacionMuerte()
{
    for (int i = 0; i < 3; i++)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var segmento in snake.Cuerpo)
        {
            Console.SetCursorPosition(segmento.X, segmento.Y);
            Console.Write("X");
        }
        Console.SetCursorPosition(snake.Cabeza.X, snake.Cabeza.Y);
        Console.Write("X");

        System.Threading.Thread.Sleep(200);

        Console.ForegroundColor = snake.ColorCuerpo;
        foreach (var segmento in snake.Cuerpo)
        {
            Console.SetCursorPosition(segmento.X, segmento.Y);
            Console.Write("■");
        }
        Console.ForegroundColor = snake.ColorCabeza;
        Console.SetCursorPosition(snake.Cabeza.X, snake.Cabeza.Y);
        Console.Write("█");

        System.Threading.Thread.Sleep(200);
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.SetCursorPosition(25, 10);
    Console.Write("¡GAME OVER!");
    Console.SetCursorPosition(22, 11);
    Console.Write($"Puntuación final: {puntuacion}");
    Console.SetCursorPosition(20, 12);
    Console.Write("Presiona cualquier tecla para continuar");
}

Iniciar();
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batalha_Naval
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Consola
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetWindowSize(180, 63);
            Console.Clear();
            #endregion

            byte vidas_1 = 0, vidas_2 = 0;                                                                          // Vidas de ambos os jogadores
            uint jogadas_para_vencer_1 = 0, jogadas_para_vencer_2 = 0;                                              // Número de jogadas de cada jogador

            byte opcao = Validacao(3, true, "\n\tIntroduza a opção que pretende:    ", 1, 3);                       // Escolha de acordo com o menu

            while(opcao != 3)                                                                                       // Ciclo de jogo que vai correr enquanto a opcao seja diferente de 3
            {
                if (opcao == 1)                                                                                     // Se opcao foi 1 entra nesta condição
                {
                    byte dificuldade = Validacao(1, true, "\n\tIndique a dificuldade que pretende:     ", 1, 3);    // Definição da dificuldade do jogo (10x10 15x15 20x20)
                    byte modo_jogo = Validacao(2, true, "\n\tIndique o modo de jogo que pretende:     ", 1, 2);     // Definição do modo de jogo (PVP ou PVE)

                    string[,] Tabuleiro_1 = GeradorTabuleiro(dificuldade);                                          // Criação do tabuleiro de barcos do jogador 1 baseado na dificuldade
                    string[,] Tabuleiro_2 = GeradorTabuleiro(dificuldade);                                          // Criação do tabuleiro de barcos do jogador 2 baseado na dificuldade
                    string[,] Tabuleiro_Tiro_1 = GeradorTabuleiro(dificuldade);                                     // Criação do tabuleiro de tiros do jogador 1 baseado na dificuldade
                    string[,] Tabuleiro_Tiro_2 = GeradorTabuleiro(dificuldade);                                     // Criação do tabuleiro de tiros do jogador 2 baseado na dificuldade

                    PreencherTabuleiro(ref Tabuleiro_1);                                                            // Preenchimento do tabuleiro de barcos do jogador 1 baseado na dificuldade
                    PreencherTabuleiro(ref Tabuleiro_2);                                                            // Preenchimento do tabuleiro de barcos do jogador 2 baseado na dificuldade
                    PreencherTabuleiro(ref Tabuleiro_Tiro_1);                                                       // Preenchimento do tabuleiro de tiros do jogador 1 baseado na dificuldade
                    PreencherTabuleiro(ref Tabuleiro_Tiro_2);                                                       // Preenchimento do tabuleiro de tiros do jogador 2 baseado na dificuldade

                    if (modo_jogo == 1)                                                                             // Modo de jogo PVP
                    {
                        Console.Write("\nIntroduza o nome do jogador 1:   ");                                       // Introdução nome do jogador_1
                        string jogador_1 = Console.ReadLine();

                        Console.Write("\nIntroduza o nome do jogador 2:   ");                                       // Introdução nome do jogador_2
                        string jogador_2 = Console.ReadLine();

                        ColocarBarcos(jogador_1, ref vidas_1, vidas_2, ref Tabuleiro_1);                            // Função para colocar barcos do jogador_1 em Tabuleiro_1
                        ColocarBarcos(jogador_2, ref vidas_2, vidas_1, ref Tabuleiro_2);                            // Função para colocar barcos do jogador_2 em Tabuleiro_2

                        while (vidas_1 > 0 && vidas_2 > 0)                                                          // Ciclo com condição de sair caso algum dos jogadores chegue ás 0 vidas e perca
                        {                                                                                           // Funções de tiro de cada jogador condicionadas por ambos terem > 0 vidas
                            if (vidas_1 > 0 && vidas_2 > 0) FuncaoTiroJogador(jogador_1, ref Tabuleiro_Tiro_1, Tabuleiro_1, ref jogadas_para_vencer_1, vidas_1, ref vidas_2, ref Tabuleiro_2);
                            if (vidas_1 > 0 && vidas_2 > 0) FuncaoTiroJogador(jogador_2, ref Tabuleiro_Tiro_2, Tabuleiro_2, ref jogadas_para_vencer_2, vidas_2, ref vidas_1, ref Tabuleiro_1);
                        }
                                                                                                                    // Função de verificação do vencedor e de escrita do highscore no ficheiro
                        VerificacaoVencedorEscrita(dificuldade, jogador_1, vidas_1, jogadas_para_vencer_1, jogador_2, vidas_2, jogadas_para_vencer_2);

                        if (vidas_1 == 0)                                                                           // Caso o vencedor seja o jogador_2
                        {
                            MostrarTabuleiro(jogador_2, vidas_2, vidas_1, false, Tabuleiro_Tiro_2);                 // Mostrar tabuleiro com vidas atualizadas
                            MostrarTabuleiro(jogador_2, vidas_2, vidas_1, true, Tabuleiro_2);                       
                        }
                        else if (vidas_2 == 0)                                                                      // Caso vencedor seja o jogador_1
                        {
                            MostrarTabuleiro(jogador_1, vidas_1, vidas_2, false, Tabuleiro_Tiro_1);                 // Mostrar tabuleiros com vidas atualizadas
                            MostrarTabuleiro(jogador_1, vidas_1, vidas_2, true, Tabuleiro_1);
                        }

                        LeituraApresentacaoFicheiro(dificuldade);                                                   // Leitura e demonstração de HIGHSCORES
                        Console.ReadKey();

                        opcao = Validacao(3, false, "\n\tIntroduza a opção que pretende:    ", 1, 3);               // Escolha da opcao novamente caso queira sair, fazer novo jogo ou ver HIGHSCORES
                        Console.ReadKey();
                    }
                    else if (modo_jogo == 2)                                                                        // Modo de jogo PVE
                    {
                        Console.Write("\nIntroduza o nome do jogador 1:   ");                                       // Introdução nome do jogador_1
                        string jogador_1 = Console.ReadLine();

                        string jogador_2 = "Computador";                                                            // Introdução nome do jogador_2 neste caso "Computador"

                        ColocarBarcos(jogador_1, ref vidas_1, vidas_2, ref Tabuleiro_1);                            // Função para colocar barcos do jogador_1 em Tabuleiro_1
                        ColocarBarcos(jogador_2, ref vidas_2, vidas_1, ref Tabuleiro_2);                            // Função para colocar barcos do jogador_2 em Tabuleiro_2

                        while (vidas_1 > 0 && vidas_2 > 0)                                                          // Ciclo com condição de sair caso algum dos jogadores chegue ás 0 vidas e perca
                        {                                                                                           // Funções de tiro de cada jogador condicionadas por ambos terem > 0 vidas
                            if (vidas_1 > 0 && vidas_2 > 0) FuncaoTiroJogador(jogador_1, ref Tabuleiro_Tiro_1, Tabuleiro_1, ref jogadas_para_vencer_1, vidas_1, ref vidas_2, ref Tabuleiro_2);
                            if (vidas_1 > 0 && vidas_2 > 0) FuncaoTiroJogador(jogador_2, ref Tabuleiro_Tiro_2, Tabuleiro_2, ref jogadas_para_vencer_2, vidas_2, ref vidas_1, ref Tabuleiro_1);
                        }
                                                                                                                    // Função de verificação do vencedor e de escrita do highscore no ficheiro
                        VerificacaoVencedorEscrita(dificuldade, jogador_1, vidas_1, jogadas_para_vencer_1, jogador_2, vidas_2, jogadas_para_vencer_2);

                        if (vidas_1 == 0)                                                                           // Caso o vencedor seja o jogador_2
                        {
                            MostrarTabuleiro(jogador_2, vidas_2, vidas_1, false, Tabuleiro_Tiro_2);                 // Mostrar tabuleiro com vidas atualizadas
                            MostrarTabuleiro(jogador_2, vidas_2, vidas_1, true, Tabuleiro_2);
                        }
                        else if (vidas_2 == 0)                                                                      // Caso o vencedor seja o jogador_1
                        {
                            MostrarTabuleiro(jogador_1, vidas_1, vidas_2, false, Tabuleiro_Tiro_1);                 // Mostrar tabuleiro com vidas atualizadas
                            MostrarTabuleiro(jogador_1, vidas_1, vidas_2, true, Tabuleiro_1);
                        }

                        LeituraApresentacaoFicheiro(dificuldade);                                                   // Leitura e demonstração de HIGHSCORES
                        Console.ReadKey();

                        opcao = Validacao(3, false, "\n\tIntroduza a opção que pretende:    ", 1, 3);               // Escolha da opcao novamente caso queira sair, fazer novo jogo ou ver HIGHSCORES
                        Console.ReadKey();
                    }
                }
                else if (opcao == 2)                                                                                // Caso a pessoa escolha que quer ver os HIGHSCORES
                {
                    byte num = Validacao(4, true, "\nIntroduza a opção que pretende:    ", 1, 3);                   // Identificação da opção pretendida de HIGHSCORES
                    LeituraApresentacaoFicheiro(num);                                                               // Leitura e demonstração de HIGHSCORES baseado na opção escolhida
                    Console.ReadKey();

                    opcao = Validacao(3, false, "\n\tIntroduza a opção que pretende:    ", 1, 3);                   // Escolha a opcao novamente caso queira sair, fazer novo jogo ou ver HIGHSCORES
                }
            }

            Console.Clear();
            Console.WriteLine("\nObrigado por jogar!\n");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Método para mostrar o Menu baseado num valor byte introduzido dependendo das situações
        static void MostrarMenu(byte menu)
        {
            if (menu == 0) Console.WriteLine();                                                                     // Se o menu for igual a 0 apenas apresentar uma linha em branco
            if (menu == 1)                                                                                          // Se o menu tiver o valor de 1
            {
                Console.WriteLine("\t _________________________________________________________________________");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|                               Dificuldade                               |");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|   1 -> Fácil (10x10)  |  2 -> Médio (15x15)  |  3 -> Difícil (20x20)    |");
                Console.WriteLine("\t|_________________________________________________________________________|");
            }
            if (menu == 2)                                                                                          // Se o menu tiver valor de 2
            {
                Console.WriteLine("\t _________________________________________________________________________");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|                              Modo de Jogo                               |");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|    1 -> Player One vs Player Two    |    2 -> Player One vs Computer    |");
                Console.WriteLine("\t|_________________________________________________________________________|");
            }
            if (menu == 3)                                                                                          // Se o menu tiver valor de 3
            {
                Console.WriteLine("\t _________________________________________________________________________");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|                              Batalha Naval                              |");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|    1 -> Começar um novo jogo  |    2 -> HIGHSCORES    |    3 -> Sair    |");
                Console.WriteLine("\t|_________________________________________________________________________|");
            }
            if (menu == 4)                                                                                          // Se o menu tiver valor de 4
            {
                Console.WriteLine("\t _________________________________________________________________________");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|                               HIGHSCORES                                |");
                Console.WriteLine("\t|                                                                         |");
                Console.WriteLine("\t|   1 -> Fácil (10x10)  |  2 -> Médio (15x15)  |  3 -> Difícil (20x20)    |");
                Console.WriteLine("\t|_________________________________________________________________________|");
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para validação de dados com parâmetros do tipo de menu, se Clear() é usado ou não, o texto introduzido antes da introdução do valor e um mínimo e máximo do valor em byte
        static byte Validacao(byte menu, bool clear, string texto, byte min, int max)
        {
            bool valido;
            byte num;

            do
            {
                if(clear == true) Console.Clear();                                                                  // Apenas caso bool clear seja true fazer Clear()
                MostrarMenu(menu);                                                                                  // Mostrar o menu com o parametro inserido na validacao
                Console.Write(texto);                                                                               // Escrever o texto de acordo com o que o usuário quiser
                if (valido = !(byte.TryParse(Console.ReadLine(), out num)) || num < min || num > max)               // Condição para mensagem de erro ser mostrada
                {
                    Console.WriteLine($"\nValor inválido, por favor indique um valor de {min} a {max}");            // Mensagem de erro
                    Console.ReadKey();
                    if(clear == true) Console.Clear();
                }
            } while (valido == true);                                                                               // Manter o ciclo ativo caso valido seja true
            return num;                                                                                             // Retorno de um valor
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para criar as matrizes que funcionam como tabuleiro de jogo cujo tamanho está baseado na variável dificuldade
        static string[,] GeradorTabuleiro(byte dificuldade)
        {
            byte linha = 0, coluna = 0;
            if (dificuldade == 1)                                                                                   // Caso a dificuldade for 1 linha e coluna receberem valor de 12
            {
                linha = 12;
                coluna = 12;
            }
            else if (dificuldade == 2)                                                                              // Caso a dificuldade for 1 linha e coluna receberem valor de 17
            {
                linha = 17;
                coluna = 17;
            }
            else if (dificuldade == 3)                                                                              // Caso a dificuldade for 1 linha e coluna receberem valor de 22
            {
                linha = 22;
                coluna = 22;
            }

            string[,] matriz = new string[linha, coluna];                                                           // Atribuição da matriz de acordo com os valores anteriores
            return matriz;                                                                                          // Retorno da matriz com o tamanho escolhido
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Método para preencher as matrizes que funcionam como tabuleiro de jogo
        static void PreencherTabuleiro(ref string[,] matriz)
        {
            for (int i = 0; i < matriz.GetLength(0); i++)                                                           // Ciclo for para percorrer as linhas da matriz
            {
                for (int j = 0; j < matriz.GetLength(1); j++)                                                       // Ciclo for para percorrer as colunas da matriz
                {                                                                                                   // Preenchimento da matriz com os números de coordenadas e com o simbolo [~]
                    if (j == 0 && i != 0 && i != matriz.GetLength(0) - 1) matriz[j, i] = $"{i}".PadLeft(3, ' ');
                    else if (j < matriz.GetLength(1) - 1 && j > 0 && i < matriz.GetLength(0) - 1) matriz[i, j] = "[~]";

                    if (j == 0 && i == 0) matriz[i, j] = ("  ");
                    else if (i == 0 && j != matriz.GetLength(1) - 1) matriz[j, i] = $"{j}".PadLeft(3, ' ');
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Método para mostrar o tabuleiro baseado nos parametros do nome de jogador, as suas vidas, as do inimigo, variável bool para diferenciar o que apresentar e a matriz que se quer apresentar
        static void MostrarTabuleiro(string jogador, byte vidas_proprio, byte vidas_enemy, bool tipo, string[,] matriz)
        {
            if (tipo == true)                                                                                       // Condição para mostrar esta secção caso tipo seja true
            {
                Console.WriteLine("\n[P][P][P][P] -> Porta-Aviões (2)\n[F][F][F] -> Fragata (2)\n[C][C] -> Corveta (2)" +
                   "\n[S] -> Submarino (2)\n[X] -> Posição Destruída\n[O] -> Água\n[p][f][c][s] -> Tipo Navio Acertado");
                Console.WriteLine($"\n\n      Tabuleiro de Navios de {jogador}\n");
            }
            else if (tipo == false)                                                                                 // Condição para mostrar esta secção caso tipo seja false
            {
                Console.WriteLine($"\nTabuleiro de Tiro de {jogador}\n");
                Console.WriteLine($"{jogador}: {vidas_proprio} vidas");                                             // Vidas do jogador
                Console.WriteLine($"Oponente: {vidas_enemy} vidas\n");                                              // Vidas do oponente
            }
            for (int i = 0; i < matriz.GetLength(0); i++)                                                           // Ciclo for para percorrer as linhas da matriz
            {
                for (int j = 0; j < matriz.GetLength(1); j++)                                                       // Ciclo for para percorrer as colunas da matriz
                {                                                                                                   // Condições para a cor de cada texto quando for printed
                    if (j == matriz.GetLength(1) - 1) Console.Write("\n");
                    if (matriz[i, j] == "[X]") Console.ForegroundColor = ConsoleColor.Red;
                    else if (matriz[i, j] == "[O]") Console.ForegroundColor = ConsoleColor.Blue;
                    else if (matriz[i, j] == "[~]" || matriz[i, j] == "   " || matriz[i, j] == "  1" || matriz[i, j] == "  2" || 
                             matriz[i, j] == "  3" || matriz[i, j] == "  4" || matriz[i, j] == "  5" || matriz[i, j] == "  6" ||
                             matriz[i, j] == "  7" || matriz[i, j] == "  8" || matriz[i, j] == "  9" || matriz[i, j] == " 10" || 
                             matriz[i, j] == " 11" || matriz[i, j] == " 12" || matriz[i, j] == " 13" || matriz[i, j] == " 14" ||
                             matriz[i, j] == " 15" || matriz[i, j] == " 16" || matriz[i, j] == " 17" || matriz[i, j] == " 18" ||
                             matriz[i, j] == " 19" || matriz[i, j] == " 20") Console.ForegroundColor = ConsoleColor.Black;
                    else if (matriz[i, j] == "[P]" || matriz[i, j] == "[F]" ||  matriz[i, j] == "[C]" || matriz[i, j] == "[S]") Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(matriz[i, j]);
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para colocar os barcos de um jogador dentro da matriz com as respectivas validações necessárias
        static void ColocarBarcos(string jogador, ref byte vidas, byte vidas_enemy, ref string[,] matriz)
        {                                                                                                    
            string[] nomes_barcos = { "Porta-Aviões", "Porta-Aviões", "Fragata", "Fragata", "Corveta", "Corveta", "Submarino", "Submarino" };
                                                                                                                    // Array com nome dos barcos
            byte[] posicoes_barcos = { 4, 4, 3, 3, 2, 2, 1, 1 };                                                    // Array com o tamanho de cada barco
            string texto = "";                                                                                      // String para introdução do texto nas coordenadas da matriz
            string[,] Tabuleiro_Clone = matriz.Clone() as string[,];                                                // Clonagem da matriz para ter uma matriz auxiliar durante a introdução de cada barco
            int coluna = 0, linha = 0;                                                                              // Valor das coordenadas
            Random gerador = new Random();

            for (int i = 0; i < nomes_barcos.Length; i++)                                                           // Ciclo que percorre cada barco baseado no tamanho do array nomes_barcos    
            {
                int[] linha_barco = new int[posicoes_barcos[i]];                                                    // Array para introdução da linha com tamanho dinâmico cada vez que mudar o barco
                int[] coluna_barco = new int[posicoes_barcos[i]];                                                   // Array para introdução da coluna com tamanho dinâmico cada vez que mudar o barco
                byte cont = 0;                                                                                      // Contador para cada vez que uma coordenada for válida
                bool option = false;                                                                                // Bool para decidir se o computador coloca o barco na vertical ou horizontal

                for (int j = 0; j < posicoes_barcos[i]; j++)                                                        // Ciclo que percorre as posições do barco que está no ciclo for exterior
                {
                    Console.Clear();
                    List<Coordinates> PosicoesVagas = new List<Coordinates>();                                      // Lista de classe Coordinates que contém todas as posições livres
                    EncontrarPosicoesLivres(ref PosicoesVagas, matriz);                                             // Preenchimento da lista anterior com as posições livres
                    MostrarTabuleiro(jogador, vidas, vidas_enemy, true, Tabuleiro_Clone);                           // Mostrar o tabuleiro clonado com os barcos inseridos até á altura
                    if (jogador == "Computador")                                                                    // Condição caso o jogador a colocar os barcos seja o computador
                    {
                        if (jogador == "Computador") System.Threading.Thread.Sleep(1000);                           // Delay de 1 segundo para proceder ao próximo passo caso o jogador seja computador
                        if (j == 0)                                                                                 // Caso a posição do barco seja a 1ª
                        {
                            int posicao_lista = gerador.Next(0, PosicoesVagas.Count);                               // Variável aleatória que vai de 0 ao tamanho da lista PosicoesVagas
                            linha = PosicoesVagas.ElementAt(posicao_lista).Linha;                                   // Atribuição de um valor de .Linha aleatório dentro da lista á variável linha
                            coluna = PosicoesVagas.ElementAt(posicao_lista).Coluna;                                 // Atribuição de um valor .Coluna aleatório dentro da lista á variável coluna
                        }
                        else if (j == 1)                                                                            // Caso a posição do barco seja a 2ª
                        {
                            if (gerador.Next(1,3) == 1)                                                             // Caso valor aleátorio entre 1 e 2 seja 1
                            {
                                coluna = coluna_barco[0];
                                linha = linha_barco[0] + 1;                                                         // Dar um valor a linha e coluna derivado da primeira coordenada do barco
                                option = true;                                                                      // Bool para na proxima coordenada identificar a direção possivel
                            }
                            else
                            {
                                coluna = coluna_barco[0] + 1;                                                       // Dar um valor a linha e coluna derivado da primeira coordenada do barco
                                linha = linha_barco[0];
                            }
                            
                        }
                        else if (j == 2)                                                                            // Caso a posição do barco seja a 3ª
                        {
                            if (option == true)                                                                     // Caso a option seja true
                            {
                                coluna = coluna_barco[1];
                                linha = linha_barco[1] + 1;
                            }
                            else
                            {                                                                                       // Caso option seja false
                                coluna = coluna_barco[1] + 1;
                                linha = linha_barco[1];
                            }
                        }
                        else if (j == 3)                                                                            // Caso a posição do barco seja a 4ª
                        {
                            if (option == true)                                                                     // Caso a option seja true
                            {
                                coluna = coluna_barco[2];
                                linha = linha_barco[2] + 1;
                            }
                            else
                            {                                                                                       // Caso a option seja false
                                coluna = coluna_barco[2] + 1;
                                linha = linha_barco[2];
                            }
                        }
                    }
                    else
                    {                                                                                               // Caso seja um jogador a introduzir os valores de linha e coluna
                        linha = Validacao(0, false, $"\nLinha da posição {j + 1} do navio nº{i + 1} - {nomes_barcos[i]}:      ", 1, matriz.GetLength(0) - 2);
                        coluna = Validacao(0, false, $"\nColuna da posição {j + 1} do navio nº{i + 1} - {nomes_barcos[i]}:        ", 1, matriz.GetLength(1) - 2);
                    }

                    coluna_barco[j] = coluna;                                                                       // Introdução da coluna no array que guarda as coordenadas de acordo com j
                    linha_barco[j] = linha;                                                                         // Introdução da linha no array que guarda as coordenadas de acordo com j
                    
                    bool valido = true;                                                                             // Bool que funcionará como portão para introduzir as coordenadas no tabuleiro clone
                    if (posicoes_barcos[i] == 1)                                                                    // Validações caso o barco tenha apenas 1 posição
                    {
                        texto = "[S]";
                        if (Tabuleiro_Clone[linha, coluna] != "[~]")
                        {
                            if (linha < 1 || linha > Tabuleiro_Clone.GetLength(0) - 2 || coluna < 1 || coluna > Tabuleiro_Clone.GetLength(1) - 2)
                            {
                                Console.WriteLine($"\nCoordenada [{linha},{coluna}] inválida. Por favor volte a introduzir de novo.");
                            }
                            else Console.WriteLine($"\nCoordenada [{linha},{coluna}] já ocupada. Por favor volte a introduzir de novo.");
                            i -= 1;
                            valido = false;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            Console.ReadKey();
                            break;
                        }
                        else if (j == 0) cont++;
                    }
                    else if (posicoes_barcos[i] == 2)                                                               // Validações caso o barco tenha 2 posições
                    {
                        texto = "[C]";
                        if (Tabuleiro_Clone[linha, coluna] != "[~]")
                        {
                            if (linha < 1 || linha > Tabuleiro_Clone.GetLength(0) - 2 || coluna < 1 || coluna > Tabuleiro_Clone.GetLength(1) - 2)
                            {
                                Console.WriteLine($"\nCoordenada [{linha},{coluna}] inválida. Por favor volte a introduzir de novo.");
                            }
                            else Console.WriteLine($"\nCoordenada [{linha},{coluna}] já ocupada. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                        else if (j == 0) cont++;
                        else if ((j == 1) &&
                                ((coluna == coluna_barco[0] && (linha == linha_barco[0] + 1 || linha == linha_barco[0] - 1)) ||
                                (linha == linha_barco[0] && (coluna == coluna_barco[0] + 1 || coluna == coluna_barco[0] - 1)))) cont++;
                        else
                        {
                            Console.WriteLine($"\nCoordenada [{linha},{coluna}] incorreta. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                    }
                    else if (posicoes_barcos[i] == 3)                                                               // Validações caso o barco tenha 3 posições
                    {
                        texto = "[F]";
                        if (Tabuleiro_Clone[linha, coluna] != "[~]")
                        {
                            if (linha < 1 || linha > Tabuleiro_Clone.GetLength(0) - 2 || coluna < 1 || coluna > Tabuleiro_Clone.GetLength(1) - 2)
                            {
                                Console.WriteLine($"\nCoordenada [{linha},{coluna}] inválida. Por favor volte a introduzir de novo.");
                            }
                            else Console.WriteLine($"\nCoordenada [{linha},{coluna}] já ocupada. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                        else if (j == 0) cont++;
                        else if ((j == 1) &&
                                ((coluna == coluna_barco[0] && (linha == linha_barco[0] + 1 || linha == linha_barco[0] - 1)) 
                                ||
                                (linha == linha_barco[0] && (coluna == coluna_barco[0] + 1 || coluna == coluna_barco[0] - 1)))) cont++;
                        else if ((j == 2) &&
                                (coluna == coluna_barco[1] && coluna == coluna_barco[0] && 
                                (linha == linha_barco[1] + 1 || linha == linha_barco[1] - 1 || linha == linha_barco[0] - 1 || linha == linha_barco[0] + 1)) 
                                ||
                                (linha == linha_barco[1] && linha == linha_barco[0] && 
                                (coluna == coluna_barco[1] + 1 || coluna == coluna_barco[1] - 1 || coluna == coluna_barco[0] - 1 || coluna == coluna_barco[0] + 1))) cont++;
                        else
                        {
                            Console.WriteLine($"\nCoordenada [{linha},{coluna}] incorreta. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                    }
                    else if (posicoes_barcos[i] == 4)                                                                   // Validações caso o barco tenha 4 posições
                    {
                        texto = "[P]";
                        if (Tabuleiro_Clone[linha, coluna] != "[~]")
                        {
                            if (linha < 1 || linha > Tabuleiro_Clone.GetLength(0) - 2 || coluna < 1 || coluna > Tabuleiro_Clone.GetLength(1) - 2)
                            {
                                Console.WriteLine($"\nCoordenada [{linha},{coluna}] inválida. Por favor volte a introduzir de novo.");
                            }
                            else Console.WriteLine($"\nCoordenada [{linha},{coluna}] já ocupada. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                        else if (j == 0) cont++;
                        else if ((j == 1) &&
                                ((coluna == coluna_barco[0] && (linha == linha_barco[0] + 1 || linha == linha_barco[0] - 1)) 
                                ||
                                (linha == linha_barco[0] && (coluna == coluna_barco[0] + 1 || coluna == coluna_barco[0] - 1)))) cont++;
                        else if ((j == 2) &&
                                (coluna == coluna_barco[1] && coluna == coluna_barco[0] && 
                                (linha == linha_barco[1] + 1 || linha == linha_barco[1] - 1 || linha == linha_barco[0] - 1 || linha == linha_barco[0] + 1)) 
                                ||
                                (linha == linha_barco[1] && linha == linha_barco[0] && 
                                (coluna == coluna_barco[1] + 1 || coluna == coluna_barco[1] - 1 || coluna == coluna_barco[0] - 1 || coluna == coluna_barco[0] + 1))) cont++;
                        else if ((j == 3) &&
                                (coluna == coluna_barco[2] && coluna == coluna_barco[1] && coluna == coluna_barco[0] && 
                                (linha == linha_barco[2] + 1 || linha == linha_barco[2] - 1 || linha == linha_barco[1] - 1 || 
                                linha == linha_barco[1] + 1 || linha == linha_barco[0] - 1 || linha == linha_barco[0] + 1)) 
                                ||
                                (linha == linha_barco[2] && linha == linha_barco[1] && linha == linha_barco[0] && 
                                (coluna == coluna_barco[2] + 1 || coluna == coluna_barco[2] - 1 || coluna == coluna_barco[1] - 1 || 
                                coluna == coluna_barco[1] + 1 ||  coluna == coluna_barco[0] - 1 || coluna == coluna_barco[0] + 1))) cont++;
                        else
                        {
                            Console.WriteLine($"\nCoordenada [{linha},{coluna}] incorreta. Por favor volte a introduzir de novo.");
                            i -= 1;
                            Tabuleiro_Clone = matriz.Clone() as string[,];
                            valido = false;
                            Console.ReadKey();
                            break;
                        }
                    }

                    if (valido == true) Tabuleiro_Clone[linha, coluna] = texto;                                         // Introdução na matriz aux com as coordenadas dadas e o texto correto

                    if (j == coluna_barco.Length - 1 && cont == coluna_barco.Length)                                    // Condição caso j esteja no penultimo caso e o contador tenha o tamanho do array coluna_barco
                    {
                        for (int k = 0; k < coluna_barco.Length; k++)                                                   // Ciclo for para percorrer e introduzir na matriz as coordenadas todas previamente dadas
                        {
                            matriz[linha_barco[k], coluna_barco[k]] = texto;
                            vidas++;
                        }                        
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para cada jogador executar o tiro onde estão englobadas todas as variáveis necessárias para o funcionamente correto nos parametros da mesma função
        static void FuncaoTiroJogador(string jogador, ref string[,] matriz_tiro, string[,] matriz_jogador, ref uint jogadas, byte vidas_proprio, ref byte vidas_enemy, ref string[,] matriz_oponente)
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        {
            int coluna = 0, linha = 0;
            bool sair = false;                                                                                          // Booleano para saber quando deve sair do ciclo pois é a vez do outro jogador
            bool tiro_certo = false;                                                                                    // Booleano para o pc saber quando acertou o último tiro
            int[] coord_pc = new int[2];                                                                                // Array para armazenar e agilizar o processo de tiro do computador

            while (sair == false)                                                                                       // Ciclo de jogada de um dos jogadores consoante for a sua vez
            {
                Console.Clear();
                MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, false, matriz_tiro);                              // Mostrar a matriz onde o jogador que tem a vez já disparou
                MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, true, matriz_jogador);                            // Mostrar a matriz onde estão os barcos do jogador que tem a vez
                if (jogador != "Computador")                                                                            // Condição para quando não for o computador a jogar
                {
                    linha = Validacao(0, false, $"\nCoordenada linha do tiro:      ", 1, matriz_tiro.GetLength(0) - 2);
                    coluna = Validacao(0, false, $"\nCoordenada coluna do tiro:        ", 1, matriz_tiro.GetLength(1) - 2);
                }
                else OpcaoTiroComputador(tiro_certo, ref linha, ref coluna, ref matriz_tiro, ref coord_pc);             // Condição para quando é o computador a jogar

                if (matriz_oponente[linha, coluna] == "[P]" || matriz_oponente[linha, coluna] == "[F]" || matriz_oponente[linha, coluna] == "[C]" || matriz_oponente[linha, coluna] == "[S]")
                {                                                                                                       // Condição de quando se acerta num dos barcos
                    Console.Clear();
                    string barco;
                    barco = matriz_oponente[linha, coluna];
                    matriz_tiro[linha, coluna] = barco.ToLower();
                    matriz_oponente[linha, coluna] = "[X]";
                    vidas_enemy--;
                    MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, false, matriz_tiro);
                    MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, true, matriz_jogador);
                    tiro_certo = true;
                    jogadas++;
                    if (vidas_enemy > 0)                                                                                // Condição caso as vidas do oponente não tenham acabado com a ultima jogada
                    {
                        Console.WriteLine($"\nAcertou num navio {barco} em [{linha},{coluna}]! Jogue outra vez.");
                        sair = false;
                    }
                    if (vidas_enemy == 0)                                                                               // Condição caso as vidas do oponente tenham acabado com a ultima jogada
                    {
                        Console.WriteLine($"\nVocê conseguiu eliminar todos os barcos do seu oponente em {jogadas} jogadas.");
                        sair = true;
                    }
                }
                else if (matriz_oponente[linha, coluna] == "[~]")                                                       // Condição para quando é tiro na água
                {
                    Console.Clear();
                    matriz_tiro[linha, coluna] = "[O]";
                    matriz_oponente[linha, coluna] = "[O]";
                    MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, false, matriz_tiro);
                    MostrarTabuleiro(jogador, vidas_proprio, vidas_enemy, true, matriz_jogador);
                    tiro_certo = false;
                    Console.WriteLine($"\n[{linha},{coluna}] é água.");
                    jogadas++;
                    sair = true;
                }
                else
                {                                                                                                       // Condição caso o tiro seja inválido
                    Console.WriteLine($"\nPosição [{linha},{coluna}] já disparada antes. Volte a introduzir as coordenadas.");
                    sair = false;                                                                            
                }
                if (jogador != "Computador") Console.ReadKey();
                if (jogador == "Computador") System.Threading.Thread.Sleep(1000);                                        // Computer sleep
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para quando for PVE onde o computador vai fazer as suas jogadas
        static void OpcaoTiroComputador(bool tiro_certo, ref int linha, ref int coluna, ref string[,] matriz_tiro, ref int[] coord_pc)
        {
            List<Coordinates> PosicoesVagas = new List<Coordinates>();                                                  // Lista onde serão armazenadas as coordenadas onde ainda não foi disparado
            EncontrarPosicoesLivres(ref PosicoesVagas, matriz_tiro);                                                    // Procedimento para encontrar as posições vagas
            Random gerador = new Random();
            bool tiro_valido = false;                                                                                   // Condição para manter-se dentro do ciclo caso o tiro seja inválido
            uint contador_falha = 0;                                                                                    // Contador para determinar quando o programa procura as coordenadas 
                                                                                                                        // vagas após acertar o último tiro e entrar em "afogamento"
            while (tiro_valido == false)                                                                                // Ciclo enquanto o tiro_valido seja false
            {
                contador_falha++;
                int posicao_lista = gerador.Next(0, PosicoesVagas.Count);                                               // Variável com uma valor aleatório de 0 até á quantidade de index dentro
                                                                                                                        // da lista PosicoesVagas para escolher um set de coordenadas vagas                
                if (tiro_certo == false)                                                                                // Caso seja o primeiro tiro
                {
                    linha = PosicoesVagas.ElementAt(posicao_lista).Linha;
                    coluna = PosicoesVagas.ElementAt(posicao_lista).Coluna;
                    coord_pc[0] = linha;
                    coord_pc[1] = coluna;
                }
                else if (tiro_certo == true)                                                                            // Caso seja o tiro após ter acertado o anterior
                {
                    gerador = new Random();
                    int num_gerado = gerador.Next(1, 5);

                    if (num_gerado == 1)
                    {
                        linha = coord_pc[0] + 1;
                        coluna = coord_pc[1];
                    }
                    else if (num_gerado == 2)
                    {
                        linha = coord_pc[0] - 1;
                        coluna = coord_pc[1];
                    }
                    else if (num_gerado == 3)
                    {
                        linha = coord_pc[0];
                        coluna = coord_pc[1] + 1;
                    }
                    else if (num_gerado == 4)
                    {
                        linha = coord_pc[0];
                        coluna = coord_pc[1] - 1;
                    }
                }
                if (contador_falha > 16)                                                                                // Quando o computador entra em "afogamento"
                {
                    linha = PosicoesVagas.ElementAt(posicao_lista).Linha;
                    coluna = PosicoesVagas.ElementAt(posicao_lista).Coluna;
                }
                if ((linha >= 1 && linha < matriz_tiro.GetLength(0) - 1) && (coluna >= 1 && coluna < matriz_tiro.GetLength(1) - 1) && matriz_tiro[linha, coluna] == "[~]")
                {                                                                                                       // Condição caso o tiro seja válido
                    tiro_valido = true;
                    coord_pc[0] = linha;
                    coord_pc[1] = coluna;
                }
                else tiro_valido = false;                                                                               // Condição caso o tiro seja inválido
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para encontrar as posições livres de terem sido jogadas por cada jogador de acordo com a matriz inserida
        static void EncontrarPosicoesLivres(ref List<Coordinates> lista, string[,] matriz_jogador)
        {
            string texto = "";
            for (int i = 0; i < matriz_jogador.GetLength(0) - 1; i++)                                                   // Ciclo for que percorre as linhas da matriz
            {
                for (int j = 0; j < matriz_jogador.GetLength(1) - 1; j++)                                               // Ciclo for que percorre as colunas da matriz
                {
                    if (matriz_jogador[i, j] == "[~]")                                                                  // Condição que indica se a posição atual percorrida está vaga ou não
                    {
                        texto = $"{i},{j}";
                        Coordinates coordenadas = new Coordinates();
                        coordenadas.Add_Coordinates(int.Parse(texto.Substring(0, texto.IndexOf(","))), int.Parse(texto.Substring(texto.IndexOf(",") + 1)));
                        lista.Add(coordenadas);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para lêr o ficheiro de HIGHSCORES e apresentá-los de acordo com a dificuldade
        static void LeituraApresentacaoFicheiro(byte dificuldade)
        {
            Console.Clear();
            string DiretorioAtual = Directory.GetCurrentDirectory().ToString();
            StreamReader leitura = new StreamReader(DiretorioAtual + @"\highscores.txt");
            List<Highscore> scores = new List<Highscore>();
            string linha = null;
            while ((linha = leitura.ReadLine()) != null)                                                                // Ciclo para fazer a leitura do ficheiro com os HIGHSCORES
            {
                Highscore linha_highscore = new Highscore();
                linha_highscore.Add_Highscore(linha.Substring(0, linha.IndexOf("-") - 1), 
                                              uint.Parse(linha.Substring(linha.IndexOf("-") + 2, (linha.IndexOf("--") - linha.IndexOf("-") - 2))), 
                                              linha.Substring(linha.IndexOf("--") + 3));
                scores.Add(linha_highscore);
            }
            if (dificuldade == 1)                                                                                       // Condição caso a dificuldade seja FÁCIL
            {
                uint cont = 1;
                Console.WriteLine("\t ___________________________________HIGHSCORES___________________________________");
                foreach (Highscore a in scores.Where(x => x.Dificuldade == "Fácil").OrderBy(x => x.Jogadas).Take(10))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\t\t\t\t\t{cont} -> {a.Nome} - {a.Jogadas} - {a.Dificuldade}");
                    cont++;
                }
                Console.WriteLine("\t ________________________________________________________________________________");
            }
            if (dificuldade == 2)                                                                                       // Condição caso a dificuldade seja MÉDIO
            {
                uint cont = 1;
                Console.WriteLine("\t ___________________________________HIGHSCORES___________________________________");
                foreach (Highscore a in scores.Where(x => x.Dificuldade == "Médio").OrderBy(x => x.Jogadas).Take(10))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\t\t\t\t\t{cont} -> {a.Nome} - {a.Jogadas} - {a.Dificuldade}");
                    cont++;
                }
                Console.WriteLine("\t ________________________________________________________________________________");
            }
            if (dificuldade == 3)                                                                                       // Condição caso a dificuldade seja DIFÍCIL
            {
                uint cont = 1;
                Console.WriteLine("\t ___________________________________HIGHSCORES___________________________________");
                foreach (Highscore a in scores.Where(x => x.Dificuldade == "Difícil").OrderBy(x => x.Jogadas).Take(10))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\t\t\t\t\t{cont} -> {a.Nome} - {a.Jogadas} - {a.Dificuldade}");
                    cont++;
                }
                Console.WriteLine("\t ________________________________________________________________________________");
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Função para verificar qual foi o vencedor e escrever o seu nome e quantas jogadas foram necessárias dentro do ficheiro HIGHSCORES
        static void VerificacaoVencedorEscrita(byte dificuldade, string jogador_1, byte vidas_1, uint jogadas_para_vencer_1, string jogador_2, byte vidas_2, uint jogadas_para_vencer_2)
        {
            string DiretorioAtual = Directory.GetCurrentDirectory().ToString();
            FileInfo highscores = new FileInfo(DiretorioAtual + "highscores.txt");
            StreamWriter escrita = highscores.AppendText();
            string nome_vencedor = "";
            uint jogadas_vencedor = 0;

            string texto_dificuldade = "";
            if (dificuldade == 1) texto_dificuldade = "Fácil";                                                          // Caso a dificuldade seja FÁCIL
            if (dificuldade == 2) texto_dificuldade = "Médio";                                                          // Caso a dificuldade seja MÉDIO
            if (dificuldade == 3) texto_dificuldade = "Difícil";                                                        // Caso a dificuldade seja DIFÍCIL

            if (vidas_1 == 0)                                                                                           // Caso vença o jogador 2
            {
                nome_vencedor = jogador_2;
                jogadas_vencedor = jogadas_para_vencer_2;

            }
            else if (vidas_2 == 0)                                                                                      // Caso vença o jogador 1
            {
                nome_vencedor = jogador_1;
                jogadas_vencedor = jogadas_para_vencer_1;
            }
            escrita.WriteLine($"{nome_vencedor} - {jogadas_vencedor} -- {texto_dificuldade}");                          // O que é escrito no ficheiro HIGHSCORES
            escrita.Close();

            Console.WriteLine($"\nParabéns {nome_vencedor}!!!");
            Console.WriteLine("\nPressione ENTER para ver o top 10 HIGHSCORES");
            Console.ReadKey();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Classe para o funcionamento das funçãos de leitura e escrita do ficheiro HIGHSCORES
    public class Highscore
    {
        public void Add_Highscore(string nome, uint jogadas, string dificuldade)                                        // Construtor da classe
        {
            Nome = nome;
            Jogadas = jogadas;
            Dificuldade = dificuldade;
        }
        public string Nome { get; set; }                                                                                // Nome do jogador
        public uint Jogadas { get; set; }                                                                               // Número de jogadas feitas
        public string Dificuldade { get; set; }                                                                         // Dificuldade jogada
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Classe para o funcionamento da função de EncontrarPosicoesLivres
    public class Coordinates
    {
        public void Add_Coordinates(int linha, int coluna)                                                              // Construtor para adicionar as coordenadas
        {
            Linha = linha;
            Coluna = coluna;
        }
        public int Linha { get; set;}                                                                                   // Linha da matriz
        public int Coluna { get; set;}                                                                                  // Coluna da matriz
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
}
using System;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Windows.Threading;

namespace ArduinoTesteSerialPort
{
	public partial class MainWindow : Window
	{
		SerialPort port;
		DispatcherTimer timer;
		byte contagem = 1;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void IniciarTimer(Double num)
		{
			timer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromMilliseconds(num)
			};
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (contagem > 0 && contagem <= 3)
			{
				mudarSinal(contagem);
			}
			else
			{
				contagem = 1;
				mudarSinal(contagem);
				contagem++;
				return;
			}
			contagem++;
		}

		#region mudando manualmente o sinal.
		private void AcenderLed_bt_Click(object sender, RoutedEventArgs e)
		{
			contagem = 1;
			mudarSinal(contagem);
		}

		private void AtencaoLed_bt_Click(object sender, RoutedEventArgs e)
		{
			contagem = 2;
			mudarSinal(contagem);
		}

		private void ApagarLed_bt_Click(object sender, RoutedEventArgs e)
		{
			contagem = 3;
			mudarSinal(contagem);
		}
		#endregion

		private void escreverTexto()
		{
			serialPort_rtb.AppendText(port.ReadLine());
		}

		private void mudarSinal(Byte num)
		{
			port.WriteLine(num.ToString());
			escreverTexto();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!(String.IsNullOrEmpty(Porta_txt.Text) || String.IsNullOrEmpty(Begin_txt.Text) || String.IsNullOrEmpty(TimerSpeed_txt.Text)))
			{
				try
				{
					port = new SerialPort(Porta_txt.Text, int.Parse(Begin_txt.Text));
					port.Open();

					if (port.IsOpen)
					{
						IniciarTimer(Convert.ToDouble(TimerSpeed_txt.Text));
						_ = MessageBox.Show("Conectado com sucesso", "Oh Yeeh", MessageBoxButton.OK, MessageBoxImage.Information);
						Elementos_gd.IsEnabled = true;
						Conectar_bt.IsEnabled = false;
						Desconectar_bt.IsEnabled = true;
						Informacoes_sp.IsEnabled = false;
					}
				}
				catch (Exception erro)
				{
					CatchException(erro);
				}
			}
			else
			{
				MessageBox.Show("Voce precisa preencher todos os campos!");
			}
		}

		private void CatchException(Exception erro)
		{
			MessageBox.Show("Tipo: " + erro.GetType().ToString() + "\n\nERRO: " + erro.Message);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Voce realmente deseja fechar a conexao?", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
			{
				MessageBox.Show("Conexao encerrada");
				Elementos_gd.IsEnabled = false;
				Conectar_bt.IsEnabled = true;
				Desconectar_bt.IsEnabled = false;
				Informacoes_sp.IsEnabled = true;
				timer.Stop();
				port.Close();
			}
		}

		private void Apitar_bt_Checked(object sender, RoutedEventArgs e)
		{
			mudarSinal(9);
		}

		private void Apitar_bt_Unchecked(object sender, RoutedEventArgs e)
		{
			mudarSinal(0);
		}

		private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Voce deseja encerrar o programa?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				App.Current.Shutdown();
			}
		}
	}
}

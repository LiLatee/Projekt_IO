   M S B A M L   `   `   `  ���� E  AIO_projekt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:IO_projekt
IO_projekt      IO_projekt.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:IO_projekt       � Title$(  "Projekt Inżynieria Oprogramowania��$	��700��$
��1200�� � 
ResizeMode$ NoResize=�.��� .+��� -    ��Name$ imageBox_generated��$
��Left=�$	��400��$��
314,96,0,0q�$	��Top=�$	��400���� -   $  imageBox_after_convolution��$	��400��$��
760,95,0,0q�$	��Top=�$
��Left=�$	��400���� $��Wygenerowany obraz��$
��Left=�$��
314,36,0,0q�$	��Top=�$��35��$	��239��$��16(��� $��Obraz po konwolucji��$
��Left=�$��
760,36,0,0q�$	��Top=�$��35��$	��239��$��16(��� $��Rozmiar (px):��$
��Left=�$��	30,98,0,0q�$	��Top=�$��30��$	��114��$��16(��� $��Ilość iteracji:��$
��Left=�$��
30,137,0,0q�$	��Top=�$��30��$	��114��$��16(��� -    ��Name$ textBox_size��$
��Left=�$��23��$��162,104,0,0q� �� TextWrapping$
 Wrap=�$
��1024��$	��Top=�$	��120��$��16(��� -   $ textBox_iterations��$
��Left=�$��23��$��162,140,0,0q�$
 Wrap=�$	��200��$	��Top=�$	��120��$��16(��� -    ��Name$ button_start��$��Wykonaj bez optymalizcji��$
��Left=�$��
30,209,0,0q�$	��Top=�$	��252��$��46��$��14(�$
��Bold"��� $
��Czas��$
��Left=�$��
28,328,0,0q�$	��Top=�$
��Bold"�$��20(�$	��252��$��Center=��� $��Bez optymalizacji:��$
��Left=�$��
28,370,0,0q�$	��Top=�$��16(�$��Right=�$	��135��$��32���� $��Z optymalizacją:��$
��Left=�$��
28,407,0,0q�$	��Top=�$��16(�$��Right=�$��32��$	��135���� $��32��$	��100��$��181,372,912.6,266.4q�._��� -    ��Name$  textBlock_synchronous_time��$
��Left=�$
��Wrap=�$��Center=�$��16(��� $�0.0 s���� $��32��$	��100��$��181,409,912.6,229.4q�._��� -   $! textBlock_asynchronous_time��$
��Left=�$
��Wrap=�$��Center=�$��16(��� $�0.0 s���� -   $ button_Start2��$��Wykonaj z optymalizcją��$
��Left=�$��
30,271,0,0q�$	��Top=�$	��252��$��46��$
��Bold"�$��14(��� -	   $ button_GenerateImage��$��Generuj losowy��$
��Left=�$��	30,52,0,0q�$	��Top=�$	��114��$��41���� $��Ilość tasków:��$
��Left=�$��
30,174,0,0q�$	��Top=�$��30��$	��114��$��16(��� -
   $ textBox_tasksCount��$
��Left=�$��23��$��162,177,0,0q�$
 Wrap=�$��8��$	��Top=�$	��120��$��16(��� -   $$ button_GenerateChessboardImage��$��Generuj szachownicę��$
��Left=�$��
168,52,0,0q�$	��Top=�$	��114��$��41���� $
��Left=�$��
30,517,0,0q�$
��Wrap=�$	��Top=�$	��116��$	��610���� $
��Bold"�$�OPTYMALIZACJA OBEJMUJE���� $�:���� �� $q�k- użycie typu float zamiast double (wartości mniejszego typu więcej zmieści się w pamięci cache cpu),���� �� $]�W- brak używania metod do przypisywania i pobierania danych (bezpośrednie odwołania),���� �� $_�Y- pobieranie danych z tablic w kolejności najlepiej wykorzystującej pamięć cache cpu,���� �� ${�u- w podwójnych pętlach działających na tablicach, wewnętrzna pętla iteruje po kolumnach (drugi indeks tablicy),���� �� $��{- pętla zawierająca więcej operacji została rozdzielona na 2 osobne pętle, aby lepiej wykorzystać pamięć cache cpu,���� �� $�- praca wielowątkowa,��
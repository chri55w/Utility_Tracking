   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      J FUtilityTracking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UtilityTrackingUtilityTracking 5      4   .UtilityTracking.User_Controls.CalculationPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      /localclr-namespace:UtilityTracking     5      -   5          >�Name$  	CalcPanel��5      .��5
      �  � RowDefinitions 5   
   �� 5      $��10 �6   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��9��6      AccentColorBrush#���   6"   $��1,10,1,1q�6S   �� 5   
   $��1��6     AccentColorBrush2#���  6!   �� 5   
   $��Calculations��6   $
��Bold"�6(   $��1��6:   $��Center=�6G   $��Center=�6d   $
��10,0q�6   �� 5   
   $��	Records: ��6   $��2��6%   $
��Left=�62   $��Center=�6M   $
��10,0q�6h   �� 5   
   -   5        $��RecordsTextBlock��5   �   $��2��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   �� *RecordsCount+ �� ElementName$ 	CalcPanel�� �� Mode$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   $��Total Cost: ��6   $��3��6(   $
��Left=�65   $��Center=�6P   $
��10,0q�6k   �� 5   
   $��3��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   ��  �� StringFormat$ 
£{0:0.##}�� �� Path$ 	TotalCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� 5   
   $��Total Payments: ��6   $��4��6,   $
��Left=�69   $��Center=�6T   $
��10,0q�6o   �� 5   
   $��4��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   �� $ 
£{0:0.##}��$ TotalPayments�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� 5    
   $��5��6   #���  6!   �� 5!   
   $��	Estimates��6   $
��Bold"�6%   $��5��67   $��Center=�6D   $��Center=�6a   $
��10,0q�6|   �� 5"   
   $��Cost Per Day: ��6   $��6��6*   $
��Left=�67   $��Center=�6R   $
��10,0q�6m   �� 5#   
   $��6��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   �� $ 
£{0:0.##}��$ 	DailyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� 5$   
   $��Cost Per Month: ��6   $��7��6,   $
��Left=�69   $��Center=�6T   $
��10,0q�6o   �� 5%   
   $��7��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   �� $ 
£{0:0.##}��$ MonthlyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� 5&   
   $��Cost Per Year: ��6   $��8��6+   $
��Left=�68   $��Center=�6S   $
��10,0q�6n   �� 5'   
   $��8��6   $��Right=�6!   $��Center=�6=   $
��10,0q�6X   ��6f   �� $ 
£{0:0.##}��$ 
YearlyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=�5(      5)      
   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  J FUtilityTracking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:UtilityTrackingUtilityTracking 4   .UtilityTracking.User_Controls.CalculationPanel   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     /localclr-namespace:UtilityTracking     -     >�Name$  	CalcPanel��.���  � RowDefinitions �� $��10 ��� �� �� �� �� �� �� �� .+��� $��9��   AccentColorBrush#���   $��1,10,1,1q��� $��1��  AccentColorBrush2#���  �� $��Calculations��$
��Bold"�$��1��$��Center=�$��Center=�$
��10,0q��� $��	Records: ��$��2��$
��Left=�$��Center=�$
��10,0q��� -   $��RecordsTextBlock��$��2��$��Right=�$��Center=�$
��10,0q����� *RecordsCount+ �� ElementName$ 	CalcPanel�� �� Mode$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��Total Cost: ��$��3��$
��Left=�$��Center=�$
��10,0q��� $��3��$��Right=�$��Center=�$
��10,0q�����  �� StringFormat$ 
£{0:0.##}�� �� Path$ 	TotalCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� $��Total Payments: ��$��4��$
��Left=�$��Center=�$
��10,0q��� $��4��$��Right=�$��Center=�$
��10,0q����� $ 
£{0:0.##}��$ TotalPayments�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� $��5��#���  �� $��	Estimates��$
��Bold"�$��5��$��Center=�$��Center=�$
��10,0q��� $��Cost Per Day: ��$��6��$
��Left=�$��Center=�$
��10,0q��� $��6��$��Right=�$��Center=�$
��10,0q����� $ 
£{0:0.##}��$ 	DailyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� $��Cost Per Month: ��$��7��$
��Left=�$��Center=�$
��10,0q��� $��7��$��Right=�$��Center=�$
��10,0q����� $ 
£{0:0.##}��$ MonthlyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=��� $��Cost Per Year: ��$��8��$
��Left=�$��Center=�$
��10,0q��� $��8��$��Right=�$��Center=�$
��10,0q����� $ 
£{0:0.##}��$ 
YearlyCost�$ 	CalcPanel��$ OneWay=�$ PropertyChanged=�
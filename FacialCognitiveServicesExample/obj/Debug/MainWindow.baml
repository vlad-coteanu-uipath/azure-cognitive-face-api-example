   M S B A M L   `   `   `  ����Y  UFacialCognitiveServicesExample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:FacialCognitiveServicesExampleFacialCognitiveServicesExample  5   	   /    )FacialCognitiveServicesExample.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   >local,clr-namespace:FacialCognitiveServicesExample     5   	     � Title$  Facial Cognitive Solution��5   	   $	��500��6+   $	��800��68   .��5	      � $��Right=�6   $��0,0,-0.4,-10q�6'   $	��794��6=   .+�5
   
   �� $����The purpose of this application is to show how powerful Azure Cognitive Services are. The example presented here is a possible part of 
an authentication module based on facial recognition and emotion detection. How does it work?

1. The user should upload 3 different images, each of the images requiring him/her to mimic a certain emotion.
2. After uploading the images, if the same user is present in all of them and the emotions are mimed correctly, then the service should
prompt that the task was successfully executed.��6   $��Center=�6U  $��30,34,30.6,0q�6r  $	��Top=�6�  $	��733��6�  $	��121��6�  �� 5   
   -   5         ��Name$ 	LeftImage��5      $
��Left=�6#   $	��134��6>   $��
30,169,0,0q�6K   $	��Top=�6_   $	��212��6w   $��/images/addImage.png��6�   �� 5   
   -   5         ��Name$ 	LeftLabel��5      $��Please upload an image��6#   $
��Left=�6D   $��
30,323,0,0q�6_   $	��Top=�6s   $	��150��6�   �� 5   
   -   5         ��Name$ 
LeftButton��5      $��Upload Photo��6%   $
��Left=�6<   $��
45,350,0,0q�6W   $	��Top=�6k   $	��100��6�   6�   �� 5   
   -   5        $ 
RightImage��5      $��Right=�6$   $	��134��6@   $��
0,169,30,0q�6M   $	��Top=�6a   $	��212��6y   $��/images/addImage.png��6�   �� 5   
   -   5        $ 
RightLabel��5      $��Please upload an image��6$   $��Right=�6E   $��
0,323,10,0q�6a   $	��Top=�6u   $	��150��6�   �� 5   
   -   5        $ RightButton��5      $��Upload Photo��6&   $��Right=�6=   $��
0,350,45,0q�6Y   $	��Top=�6m   $	��100��6�   6�   �� 5   
   -   5        $ CenterImage��5      $��Center=�6%   $	��134��6B   $��291,169,291,0q�6O   $	��Top=�6f   $	��212��6~   $��/images/addImage.png��6�   �� 5   
   -   5        $ CenterLabel��5      $��Please upload an image��6%   $��Center=�6F   $��322,323,322,0q�6c   $	��Top=�6z   $	��150��6�   �� 5   
   -	   5        $ CenterButton��5      $��Upload Photo��6'   $��Center=�6>   $��	0,350,0,0q�6[   $	��Top=�6n   $	��100��6�   6�   �� 5   
   -
   5        $ ResetButton��5      $��Reset images��6&   $��Center=�6=   $��291,390,291,0q�6Z   $	��Top=�6q   $	��212��6�   6�   �� 5   
   -   5        $ ValidateButton��5      $��Validate��6)   $��Center=�6<   $��291,419,291,0q�6Y   $	��Top=�6p   $	��212��6�   6�   5      5      
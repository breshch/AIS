using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoRemainsViewModel : ViewModelGlobal
    {

    }
}
 //<StackPanel Grid.Column="0" Orientation="Horizontal">
 //               <TextBlock Text="Артикул" Margin="0,5,0,5"/>
 //               <ComboBox DataContext="{Binding Article}" SelectedItem="{Binding SelectedArticle}" IsEditable="True" Width="150" Margin="20, 0, 0, 0"/>
 //           </StackPanel>
            
 //           <StackPanel Grid.Column="1" Orientation="Horizontal" Margin="20,0,0,0">
 //               <TextBlock Text="Дата"  Margin="0,5,0,5"/>
 //               <DatePicker SelectedDate="{Binding SelectedDate, Mode=TwoWay}"  Width="150" Margin="20, 0, 0, 0"/> 
 //           </StackPanel> 

 //       </Grid>
 //       <Grid Grid.Row="1" Margin="0,20,0,0">
            
 //           <DataGrid AutoGenerateColumns="False" HeadersVisibility="Column" SelectionMode="Single" SelectionUnit="FullRow" IsReadOnly="True"
 //                     CanUserAddRows="False" CanUserDeleteRows="False" CanUserResizeColumns="False" CanUserResizeRows="False" ItemsSource="{Binding InfoCarPartRemain}" SelectedItem="{Binding SelectedCarPartRemain}">>
            
 //           <DataGrid.Columns>
 //                   <DataGridTextColumn Header="Остаток на 1-ое число" Binding="{Binding FirstRemains }" Width="*"/>
 //                   <DataGridTextColumn Header="Всего приходов за месяц" Binding="{Binding Incommings}" Width="*"/>
 //                   <DataGridTextColumn Header="Всего расходов за месяц" Binding="{Binding Outcomes}" Width="*"/>
 //               <DataGridTextColumn Header="Всего остаток на складе " Binding="{Binding RemainToDate}" Width="*"/>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorteRO.ascx.cs" Inherits="SAM.Web.Controles.Ingenieria.CorteRO"  EnableViewState="false"%>
<br />
<div class="infoCorte soloLectura ancho100" >
  
      
<div class="divIzquierdo ancho50" style="overflow:hidden ; margin-top:15px">

    <div class="ancho100" id="bdCorte" style="margin-left:5px">        
        <asp:Repeater runat="server" ID="repCortesBD" OnItemDataBound="repCortesBD_OnItemDataBound">            
            <HeaderTemplate>                
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="8%" />
                        <col width="15%" />
                        <col width="25%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="*" />
                    </colgroup>                   
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="9"><asp:Literal runat="server" ID="litCortes" meta:resourcekey="litCortes" />&nbsp;<asp:Literal runat="server" ID="litRevisionBD" /></th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                            <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                            <th><asp:Literal runat="server" ID="litDiametro" meta:resourcekey="litDiametro" /></th>
                            <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                            <th><asp:Literal runat="server" ID="litSegmento" meta:resourcekey="litSegmento" /></th>
                            <th><asp:Literal runat="server" ID="litInicioFin" meta:resourcekey="litInicioFin" /></th>
                            <th><asp:Literal runat="server" ID="litTipoCorte1" meta:resourcekey="litTipoCorte1" /></th>
                            <th><asp:Literal runat="server" ID="litTipoCorte2" meta:resourcekey="litTipoCorte2" /></th>
                        </tr>
                    </thead>
            </HeaderTemplate>

            <ItemTemplate>
                    <asp:Literal runat="server" ID="litTrAbre" />                    
                        <asp:Literal runat="server" ID="litEtiquetaMaterial" />
                        <asp:Literal runat="server" ID="litCodigoItemCode"/>
                        <asp:Literal runat="server" ID="litDescripcionItemCode"/>
                        <asp:Literal runat="server" ID="litDiametro"/>
                        <asp:Literal runat="server" ID="litCantidad"/>
                        <asp:Literal runat="server" ID="litEtiquetaSeccion"/>
                        <asp:Literal runat="server" ID="litInicioFin"/>
                        <asp:Literal runat="server" ID="litTipoCorte1"/>
                        <asp:Literal runat="server" ID="litTipoCorte2"/>
                    </tr>                    
            </ItemTemplate>  
                      
            <FooterTemplate>                    
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="9">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>                
            </FooterTemplate>            
        </asp:Repeater>        
    </div> 

    

</div>
 
 
<div class="divIzquierdo ancho50" style="margin-top: 15px;overflow:hidden; ">
    
    <div class="ancho100" id="archivoCorte" style="margin-left:5px">        
        <asp:Repeater runat="server" ID="repCortesArchivo" OnItemDataBound="repCortesArchivo_OnItemDataBound">
            <HeaderTemplate>
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="8%" />
                        <col width="15%" />
                        <col width="25%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="8%" />
                        <col width="*" />
                    </colgroup>  
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="9"><asp:Literal runat="server" ID="Literal1" meta:resourcekey="litCortes" />&nbsp;<asp:Literal runat="server" ID="litRevisionArchivo" /></th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="Literal3" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="Literal4" meta:resourcekey="litIcCodigo" /></th>
                            <th><asp:Literal runat="server" ID="Literal5" meta:resourcekey="litIcDescripcion" /></th>
                            <th><asp:Literal runat="server" ID="Literal6" meta:resourcekey="litDiametro" /></th>
                            <th><asp:Literal runat="server" ID="Literal7" meta:resourcekey="litCantidad" /></th>
                            <th><asp:Literal runat="server" ID="Literal8" meta:resourcekey="litSegmento" /></th>
                            <th><asp:Literal runat="server" ID="Literal9" meta:resourcekey="litInicioFin" /></th>
                            <th><asp:Literal runat="server" ID="Literal10" meta:resourcekey="litTipoCorte1" /></th>
                            <th><asp:Literal runat="server" ID="Literal11" meta:resourcekey="litTipoCorte2" /></th>
                        </tr>
                    </thead>                  
            </HeaderTemplate>

            <ItemTemplate>
                    <asp:Literal runat="server" ID="litTrAbre" />                    
                        <asp:Literal runat="server" ID="litEtiquetaMaterial" />
                        <asp:Literal runat="server" ID="litCodigoItemCode"/>
                        <asp:Literal runat="server" ID="litDescripcionItemCode"/>
                        <asp:Literal runat="server" ID="litDiametro"/>
                        <asp:Literal runat="server" ID="litCantidad"/>
                        <asp:Literal runat="server" ID="litEtiquetaSeccion"/>
                        <asp:Literal runat="server" ID="litInicioFin"/>
                        <asp:Literal runat="server" ID="litTipoCorte1"/>
                        <asp:Literal runat="server" ID="litTipoCorte2"/>
                    </tr>                                
            </ItemTemplate>   
                        
            <FooterTemplate> 
                     <tfoot>
                        <tr class="repPie">
                            <td colspan="9">&nbsp;</td>
                        </tr>
                    </tfoot>                   
                </table>
            </FooterTemplate>
        </asp:Repeater>
        
    </div>
    
    <p></p> 

</div>

    

        
</div>

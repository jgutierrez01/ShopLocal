<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialRO.ascx.cs" Inherits="SAM.Web.Controles.Ingenieria.MaterialRO"  %>
<div class="infoJuntas soloLectura ancho100" style="">

<asp:HiddenField ID="hidSpoolId" ClientIDMode="Static" Value="" runat="server"  />
<asp:HiddenField ID="hidSpoolPendienteId" ClientIDMode="Static" Value="" runat="server"  />

<asp:HiddenField ID="hidMaterialSpoolPendienteId" ClientIDMode="Static" Value="" runat="server"  />
<asp:HiddenField ID="hidMaterialSpoolId" ClientIDMode="Static" Value="" runat="server"  />

<asp:HiddenField ID="hidAccion" ClientIDMode="Static" Value="" runat="server"  />

<asp:Panel id="divError" runat="server" CssClass="summaryList" Visible="false" >
 <asp:Literal runat="server" meta:resourcekey="litTituloError" />  
<ul>
    <li>
        <asp:Literal ID="litError" runat="server" />        
    </li>
</ul>
    
</asp:Panel>

<div class="ancho100" style="text-align:center">
    <asp:Button ID="btnIgual" runat="server" Text="=" CssClass="boton" OnClientClick="javascript: return Sam.Ingenieria.HomologacionValidaOperacion(0)"  OnClick="btnIgual_OnClick"/>
    <asp:Button ID="btnNuevo" runat="server" Text="N" CssClass="boton" OnClientClick="javascript: return Sam.Ingenieria.HomologacionValidaOperacion(1)" OnClick="btnNuevo_OnClick"/>
    <asp:Button ID="btnEliminar" runat="server" Text="X" CssClass="boton" OnClientClick="javascript: return Sam.Ingenieria.HomologacionValidaOperacion(2)" OnClick="btnEliminar_OnClick"/>
    <asp:Button ID="btnSimilar" runat="server" Text="≈" CssClass="boton" OnClientClick="javascript: return Sam.Ingenieria.HomologacionValidaOperacion(3)" OnClick="btnSimilar_OnClick"/>    

    
</div>


<div class="ancho50 divIzquierdo" style="overflow:hidden; margin-top:15px"> 
   <div class="ancho100 homologacionTabla" id="bdMat" >           
        <asp:Repeater runat="server" ID="repMaterialesBD" OnItemDataBound="repMaterialesBD_OnItemDataBound">
            <HeaderTemplate>                
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="7%" />
                        <col width="10%" />
                        <col width="21%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="13%" />
                        <col width="10%" />
                        <col width="*" />
                    </colgroup> 
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="9"><asp:Literal runat="server" ID="Literal1" meta:resourcekey="litMateriales" />&nbsp;<asp:Literal runat="server" ID="litRevisionBD" /></th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="Literal7" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="Literal2" meta:resourcekey="litIcCodigo" /></th>
                            <th><asp:Literal runat="server" ID="Literal3" meta:resourcekey="litIcDescripcion" /></th>
                            <th><asp:Literal runat="server" ID="Literal4" meta:resourcekey="litD1" /></th>
                            <th><asp:Literal runat="server" ID="Literal5" meta:resourcekey="litD2" /></th>
                            <th><asp:Literal runat="server" ID="Literal6" meta:resourcekey="litCantidad" /></th>                    
                            <th><asp:Literal runat="server" ID="Literal8" meta:resourcekey="litCategoria" /></th>
                            <th><asp:Literal runat="server" ID="Literal9" meta:resourcekey="litEspecificacion" /></th>
                            <th><asp:Literal runat="server" ID="Literal10" meta:resourcekey="litKg" /></th>
                        </tr>
                    </thead>                  
            </HeaderTemplate>

            <ItemTemplate>
                    <asp:Literal runat="server" ID="litTrAbre" />                                            
                        <asp:Literal runat="server" ID="litEtiqueta"/>
                        <asp:Literal runat="server" ID="litCodigoItemCode"/>
                        <asp:Literal runat="server" ID="litDescripcionItemCode"/>
                        <asp:Literal runat="server" ID="litDiametro1"/>
                        <asp:Literal runat="server" ID="litDiametro2"/>
                        <asp:Literal runat="server" ID="litCantidad"/>                        
                        <asp:Literal runat="server" ID="litCategoria"/>
                        <asp:Literal runat="server" ID="litEspecificacion"/>
                        <asp:Literal runat="server" ID="litPeso"/>
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

<div class="ancho50 divIzquierdo" style="overflow:hidden;margin-top: 15px;">
    
    <div class="ancho100 homologacionTabla" id="archivoMat"  style="margin-left: 5px;" >           
        <asp:Repeater runat="server" ID="repMaterialesArchivo" OnItemDataBound="repMaterialesArchivo_OnItemDataBound">
            <HeaderTemplate>                
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="7%" />
                        <col width="10%" />
                        <col width="21%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="10%" />
                        <col width="13%" />
                        <col width="10%" />
                        <col width="*" />
                    </colgroup>   
                    <thead>
                    <tr class="repEncabezado">
                        <th colspan="9"><asp:Literal runat="server" ID="Literal11" meta:resourcekey="litMateriales" />&nbsp;<asp:Literal runat="server" ID="litRevisionArchivo" /></th>
                    </tr>
                    <tr class="repTitulos">
                        <th><asp:Literal runat="server" ID="Literal17" meta:resourcekey="litEtiqueta" /></th>
                        <th><asp:Literal runat="server" ID="Literal12" meta:resourcekey="litIcCodigo" /></th>
                        <th><asp:Literal runat="server" ID="Literal13" meta:resourcekey="litIcDescripcion" /></th>
                        <th><asp:Literal runat="server" ID="Literal14" meta:resourcekey="litD1" /></th>
                        <th><asp:Literal runat="server" ID="Literal15" meta:resourcekey="litD2" /></th>
                        <th><asp:Literal runat="server" ID="Literal16" meta:resourcekey="litCantidad" /></th>
                        <th><asp:Literal runat="server" ID="Literal18" meta:resourcekey="litCategoria" /></th>
                        <th><asp:Literal runat="server" ID="Literal19" meta:resourcekey="litEspecificacion" /></th>
                        <th><asp:Literal runat="server" ID="Literal20" meta:resourcekey="litKg" /></th>
                    </tr>
                </thead>                
            </HeaderTemplate>

            <ItemTemplate>
                    <asp:Literal runat="server" ID="litTrAbre" />                                            
                        <asp:Literal runat="server" ID="litEtiqueta"/>
                        <asp:Literal runat="server" ID="litCodigoItemCode"/>
                        <asp:Literal runat="server" ID="litDescripcionItemCode"/>
                        <asp:Literal runat="server" ID="litDiametro1"/>
                        <asp:Literal runat="server" ID="litDiametro2"/>
                        <asp:Literal runat="server" ID="litCantidad"/>
                        <asp:Literal runat="server" ID="litCategoria"/>
                        <asp:Literal runat="server" ID="litEspecificacion"/>
                        <asp:Literal runat="server" ID="litPeso"/>
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

<div class="ancho100">
    <img src="../../Imagenes/Iconos/ico_admiracion.png" />&nbsp;&nbsp;<asp:Literal ID="Literal21" meta:resourcekey="litLeyendaMaterial" runat="server"></asp:Literal>
</div>

<div class="ancho100 homologacionTabla" id="divResultante" style="text-align:center" >           
    <asp:Repeater runat="server" ID="repMaterialesResultado" OnItemDataBound="repMaterialesResultado_OnItemDataBound">
        <HeaderTemplate>                
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="7%" />
                    <col width="10%" />
                    <col width="21%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="13%" />
                    <col width="10%" />
                    <col width="*" />
                </colgroup> 
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="9"><asp:Literal runat="server" ID="Literal1" meta:resourcekey="litMateriales" />&nbsp;<asp:Literal runat="server" ID="litRevisionBD" /></th>
                    </tr>
                    <tr class="repTitulos">
                        <th><asp:Literal runat="server" ID="Literal7" meta:resourcekey="litEtiqueta" /></th>
                        <th><asp:Literal runat="server" ID="Literal2" meta:resourcekey="litIcCodigo" /></th>
                        <th><asp:Literal runat="server" ID="Literal3" meta:resourcekey="litIcDescripcion" /></th>
                        <th><asp:Literal runat="server" ID="Literal4" meta:resourcekey="litD1" /></th>
                        <th><asp:Literal runat="server" ID="Literal5" meta:resourcekey="litD2" /></th>
                        <th><asp:Literal runat="server" ID="Literal6" meta:resourcekey="litCantidad" /></th>                    
                        <th><asp:Literal runat="server" ID="Literal8" meta:resourcekey="litCategoria" /></th>
                        <th><asp:Literal runat="server" ID="Literal9" meta:resourcekey="litEspecificacion" /></th>
                        <th><asp:Literal runat="server" ID="Literal10" meta:resourcekey="litKg" /></th>
                    </tr>
                </thead>                  
        </HeaderTemplate>

        <ItemTemplate>
                <asp:Literal runat="server" ID="litTrAbre" />                                            
                    <asp:Literal runat="server" ID="litEtiqueta"/>
                    <asp:Literal runat="server" ID="litCodigoItemCode"/>
                    <asp:Literal runat="server" ID="litDescripcionItemCode"/>
                    <asp:Literal runat="server" ID="litDiametro1"/>
                    <asp:Literal runat="server" ID="litDiametro2"/>
                    <asp:Literal runat="server" ID="litCantidad"/>                        
                    <asp:Literal runat="server" ID="litCategoria"/>
                    <asp:Literal runat="server" ID="litEspecificacion"/>
                    <asp:Literal runat="server" ID="litPeso"/>
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

<script type="text/javascript">    
    $('.simplehighlight').click(function () {
        if ($(this).hasClass("homologacionSelected")) {
            $(this).removeClass('homologacionSelected');
            return
        }

        $(this).parent().children().removeClass('homologacionSelected');
        $(this).addClass('homologacionSelected');
    });
    
</script>
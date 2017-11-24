<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JuntaRO.ascx.cs" Inherits="SAM.Web.Controles.Ingenieria.JuntaRO" EnableViewState="false" %>
<div class="infoJunta soloLectura ancho100" >
 
<div class="ancho50 divIzquierdo" style="overflow:hidden ; margin-top:15px">
    <div class="ancho100" id="headersBDJunta" style="margin-left:5px" >
            <asp:Repeater runat="server" ID="repJuntasBD" OnItemDataBound="repJuntasBD_OnItemDataBound">
                <HeaderTemplate>                
                    <table class="repSam" cellpadding="0" cellspacing="0">
                        <colgroup>                        
                            <col width="13%" />
                            <col width="13%" />
                            <col width="10%" />
                            <col width="15%" />
                            <col width="17%" />
                            <col width="17%" />
                            <col width="10%" />
                            <col width="*" />                                       
                        </colgroup>  
                        <thead>
                        <tr class="repEncabezado">
                            <th colspan="7"><asp:Literal runat="server" ID="litEncJuntas" meta:resourcekey="litJuntas" />&nbsp;<asp:Literal runat="server" ID="litRevisionBD" /></th>
                        </tr>
                        <tr class="repTitulos">                    
                            <th><asp:Literal runat="server" ID="Literal1" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="Literal2" meta:resourcekey="litDiametro" /></th>
                            <th><asp:Literal runat="server" ID="Literal13" meta:resourcekey="litTipoJunta" /></th>
                            <th><asp:Literal runat="server" ID="Literal14" meta:resourcekey="litCedula" /></th>
                            <th><asp:Literal runat="server" ID="Literal15" meta:resourcekey="litLocalizacion" /></th>
                            <th><asp:Literal runat="server" ID="Literal16" meta:resourcekey="litFamiliaAcero" /></th>
                            <th><asp:Literal runat="server" ID="Literal17" meta:resourcekey="litFabArea" /></th>                    
                            <th><asp:Literal runat="server" ID="Literal11" meta:resourcekey="litRequierePwht" /></th>  
                        </tr>
                    </thead>                 
                </HeaderTemplate>

                <ItemTemplate>            
                        <asp:Literal runat="server" ID="litTrAbre" />  
                            <asp:Literal runat="server" ID="litEtiqueta" />                                     
                            <asp:Literal runat="server" ID="litDiametro"/>
                            <asp:Literal runat="server" ID="litTipoJunta"/>
                            <asp:Literal runat="server" ID="litCedula"/>
                            <asp:Literal runat="server" ID="litLocalizacion"/>
                            <asp:Literal runat="server" ID="litFamiliasAcero"/>
                            <asp:Literal runat="server" ID="litFabArea"/>                                               
                            <asp:Literal runat="server" ID="litRequierePwht"/>    
                        </tr>                    
                </ItemTemplate>
                            
                <FooterTemplate>
                        <tfoot>
                            <tr class="repPie">
                                <td colspan="8">&nbsp;</td>
                            </tr>
                        </tfoot>                    
                    </table>                
                </FooterTemplate>    
                        
            </asp:Repeater>    

        </div>

        
</div>
        
<div class="ancho50 divIzquierdo" style="overflow:hidden ; margin-top:15px">
    

    <div class="ancho100" id="archivoJunta" style="margin-left:5px">        
        <asp:Repeater runat="server" ID="repJuntasArchivo" OnItemDataBound="repJuntasArchivo_OnItemDataBound">
            <HeaderTemplate>
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>                        
                        <col width="13%" />
                        <col width="13%" />
                        <col width="10%" />
                        <col width="15%" />
                        <col width="17%" />
                        <col width="17%" />
                        <col width="10%" />                                        
                        <col width="*" />     
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="7"><asp:Literal runat="server" ID="Literal3" meta:resourcekey="litJuntas" />&nbsp;<asp:Literal runat="server" ID="litRevisionArchivo" /></th>
                        </tr>
                        <tr class="repTitulos">  
                            <th><asp:Literal runat="server" ID="Literal4" meta:resourcekey="litEtiqueta" /></th>                  
                            <th><asp:Literal runat="server" ID="Literal5" meta:resourcekey="litDiametro" /></th>
                            <th><asp:Literal runat="server" ID="Literal6" meta:resourcekey="litTipoJunta" /></th>
                            <th><asp:Literal runat="server" ID="Literal7" meta:resourcekey="litCedula" /></th>
                            <th><asp:Literal runat="server" ID="Literal8" meta:resourcekey="litLocalizacion" /></th>
                            <th><asp:Literal runat="server" ID="Literal9" meta:resourcekey="litFamiliaAcero" /></th>
                            <th><asp:Literal runat="server" ID="Literal10" meta:resourcekey="litFabArea" /></th>                    
                            <th><asp:Literal runat="server" ID="Literal12" meta:resourcekey="litRequierePwht" /></th>  
                        </tr>
                    </thead>                    
            </HeaderTemplate>

            <ItemTemplate>
                    <asp:Literal runat="server" ID="litTrAbre" />    
                        <asp:Literal runat="server" ID="litEtiqueta" />
                        <asp:Literal runat="server" ID="litDiametro"/>
                        <asp:Literal runat="server" ID="litTipoJunta"/>
                        <asp:Literal runat="server" ID="litCedula"/>
                        <asp:Literal runat="server" ID="litLocalizacion"/>
                        <asp:Literal runat="server" ID="litFamiliasAcero"/>
                        <asp:Literal runat="server" ID="litFabArea"/>                        
                        <asp:Literal runat="server" ID="litRequierePwht"/>  
                    </tr>                           
            </ItemTemplate> 
                          
            <FooterTemplate> 
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="8">&nbsp;</td>
                        </tr>
                    </tfoot>                   
                </table>
            </FooterTemplate>
        </asp:Repeater>
        
    </div>
    
</div>    

    
    <p></p> 
    <img src="../../Imagenes/Iconos/ico_admiracion.png" /> &nbsp;&nbsp;<asp:Literal ID="litLeyendaJunta" meta:resourcekey="litLeyendaJunta" runat="server"></asp:Literal>
</div>
<script type="text/javascript">
   
</script>

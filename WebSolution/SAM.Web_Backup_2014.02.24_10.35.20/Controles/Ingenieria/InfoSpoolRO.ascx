<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoSpoolRO.ascx.cs" Inherits="SAM.Web.Controles.Ingenieria.InfoSpoolRO" EnableViewState="false" %>
<div class="infoSpool soloLectura ancho100" style="margin-left:30px">
        

    <div class="divIzquierdo ancho40 homologacion" id="headersBDSpool" style="overflow:hidden ; margin-top:15px">
        <table class="repSam" cellpadding="0" cellspacing="0">
            <colgroup>                
                <col width="190" />                
                <col width="190" />
            </colgroup>
                <tr class="repEncabezado">
                    <th colspan="2"><asp:Literal runat="server" ID="litEncSpool" meta:resourcekey="litSpool" />&nbsp;<asp:Literal runat="server" ID="litRevisionBD" /></th>                    
                </tr>
                <tr class="repTitulos">                                        
                    <th><asp:Literal runat="server" ID="Literal12" meta:resourcekey="litDibujo" /></th>
                   <asp:Literal runat="server" ID="litDibujoBD" />
                </tr>
                <tr class="repTitulos">    
                    <th><asp:Literal runat="server" ID="Literal1" meta:resourcekey="litCedula" /></th>
                    <asp:Literal runat="server" ID="litCedulaBD"/>    
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal2" meta:resourcekey="litNombre" /></th>
                     <asp:Literal runat="server" ID="litNombreBD"/>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal13" meta:resourcekey="litPdi" /></th>
                    <asp:Literal runat="server" ID="litPdiBD"/>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal14" meta:resourcekey="litPeso" /></th>
                    <asp:Literal runat="server" ID="litPesoBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal15" meta:resourcekey="litArea" /></th>
                    <asp:Literal runat="server" ID="litAreaBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal16" meta:resourcekey="litEspecificacion" /></th>
                    <asp:Literal runat="server" ID="litEspecificacionBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal17" meta:resourcekey="litPnd" /></th>
                    <asp:Literal runat="server" ID="litPndBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal11" meta:resourcekey="litPwht" /></th>                                        
                    <asp:Literal runat="server" ID="litPwhtBD"  />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal19" meta:resourcekey="litDiametro" /></th>
                    <asp:Literal runat="server" ID="litDiametroBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal18" meta:resourcekey="litRevCte" /></th>
                    <asp:Literal runat="server" ID="litRevCteBD" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal24" meta:resourcekey="litRevSt" /></th>
                    <asp:Literal runat="server" ID="litRevStBD" />
                </tr>
        </table>
    </div>
    

    <div class="divIzquierdo ancho40 homologacion" id="headersArchivoSpool" style="overflow:hidden; margin-top:15px; margin-left:50px">
        <table class="repSam" cellpadding="0" cellspacing="0">
            <colgroup>                
                <col width="190" />                
                <col width="190" />                         
            </colgroup>
                <tr class="repEncabezado">
                    <th colspan="12"><asp:Literal runat="server" ID="Literal3" meta:resourcekey="litSpool" />&nbsp;<asp:Literal runat="server" ID="litRevisionArchivo" /></th>
                </tr>
                <tr class="repTitulos">                                        
                    <th><asp:Literal runat="server" ID="Literal4" meta:resourcekey="litDibujo" /></th>
                    <asp:Literal runat="server" ID="litDibujoAR" />
                </tr>
                <tr class="repTitulos">    
                    <th><asp:Literal runat="server" ID="Literal6" meta:resourcekey="litCedula" /></th>
                    <asp:Literal runat="server" ID="litCedulaAR"/>    
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal8" meta:resourcekey="litNombre" /></th>
                     <asp:Literal runat="server" ID="litNombreAR"/>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal10" meta:resourcekey="litPdi" /></th>
                    <asp:Literal runat="server" ID="litPdiAR"/>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal21" meta:resourcekey="litPeso" /></th>
                    <asp:Literal runat="server" ID="litPesoAR" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal23" meta:resourcekey="litArea" /></th>
                    <asp:Literal runat="server" ID="litAreaAR" />
                </tr>
                <tr class="repTitulos">
                    <th>
                    <asp:Literal runat="server" ID="Literal26" meta:resourcekey="litEspecificacion" />
                    </th>
                    <asp:Literal runat="server" ID="litEspecificacionAR" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal28" meta:resourcekey="litPnd" /></th>
                    <asp:Literal runat="server" ID="litPndAR" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal30" meta:resourcekey="litPwht" /></th>                                        
                    <asp:Literal runat="server" ID="litPwhtAR"  />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal32" meta:resourcekey="litDiametro" /></th>
                    <asp:Literal runat="server" ID="litDiametroAR" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal34" meta:resourcekey="litRevCte" /></th>
                    <asp:Literal runat="server" ID="litRevCteAR" />
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="Literal36" meta:resourcekey="litRevSt" /></th>
                    <asp:Literal runat="server" ID="litRevStAR" />
                </tr>
        </table>
    </div>

    
    

</div>

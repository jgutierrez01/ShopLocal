IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GED_VISTA]') AND type in (N'P', N'PC'))
	DROP VIEW [dbo].[GED_VISTA]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		GED_VISTA
	Funcion:	Trae toda la informacion para Juntas finales
	Autor:		MMG
	Modificado: 04/04/2011 MMG
*****************************************************************************************/

CREATE VIEW [dbo].[GED_VISTA]
AS
	select P.ProyectoID [id_project],
		   SP.Nombre [spool],
		   SP.Dibujo [iso],
		   JSP.Etiqueta [joint],
		   JWST.EtiquetaJunta [state],
		   JSP.Cedula [schedule],
		   PRORE.Nombre + '/' + PRORA.Nombre [coladas],
		   WPS.Nombre [wps],
		   sra.SoldadorRaiz [root_w],
		   sr.SoldadorRelleno [filler_w],
		   INV.NumeroReporte [visual_report],
		   JPND.ReportesPND [pnd],
		   JTT.ReportesTT [pwht_hard]
	from Proyecto P
		INNER JOIN Spool SP
			on SP.ProyectoID = P.ProyectoID
		INNER JOIN JuntaSpool JSP
			on JSP.SpoolID = SP.SpoolID
		INNER JOIN JuntaWorkstatus JWST
			on JWST.JuntaSpoolID = JSP.JuntaSpoolID and JWST.JuntaFinal = '1'
		LEFT JOIN JuntaSoldadura JSOL
			on JSOL.JuntaSoldaduraID = JWST.JuntaSoldaduraID
		LEFT JOIN ProcesoRaiz PRORA
			on PRORA.ProcesoRaizID = JSOL.ProcesoRaizID
		LEFT JOIN ProcesoRelleno PRORE
			on PRORE.ProcesoRellenoID = JSOL.ProcesoRellenoID
		LEFT JOIN Wps WPS
			on WPS.WpsID = JSOL.WpsID		
		LEFT JOIN InspeccionVisual INV
			on INV.InspeccionVisualID = JWST.JuntaInspeccionVisualID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = JSOL.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sra on sra.JuntaSoldaduraID = JSOL.JuntaSoldaduraID
		LEFT JOIN(
			SELECT JRPND.JuntaWorkstatusID, 
				   REPLACE(RTRIM((SELECT RPND.NumeroReporte + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePnd JRPND1
			INNER JOIN ReportePnd RPND on RPND.ReportePndID = JRPND1.ReportePndID
			WHERE (JRPND1.JuntaWorkstatusID = JRPND.JuntaWorkstatusID)
			FOR XML PATH (''))),' ','/') AS ReportesPND
			FROM JuntaReportePnd JRPND
			GROUP BY JRPND.JuntaWorkstatusID
		) JPND on JPND.JuntaWorkstatusID = JWST.JuntaWorkstatusID
		LEFT JOIN(
			SELECT JRTT.JuntaWorkstatusID, 
				   REPLACE(RTRIM((SELECT RTT.NumeroReporte + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReporteTT JRTT1
			INNER JOIN ReporteTT RTT on RTT.ReporteTtID = JRTT1.ReporteTtID
			WHERE (JRTT1.JuntaWorkstatusID = JRTT.JuntaWorkstatusID) and RTT.TipoPruebaID in ('3','4')
			FOR XML PATH (''))),' ','/') AS ReportesTT
			FROM JuntaReporteTT JRTT
			GROUP BY JRTT.JuntaWorkstatusID
		) JTT on JTT.JuntaWorkstatusID = JWST.JuntaWorkstatusID
		
	

GO



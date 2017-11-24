Use Sam
GO

ALTER TABLE [dbo].[JuntaSoldadura] ADD WpsRellenoID int
GO

ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_WpsRelleno] FOREIGN KEY([WpsRellenoID])
REFERENCES [dbo].[Wps] ([WpsID])
GO
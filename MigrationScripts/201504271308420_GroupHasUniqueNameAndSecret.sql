﻿ALTER TABLE [dbo].[Groups] ADD [Secret] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Groups] ALTER COLUMN [Name] [nvarchar](100) NOT NULL
CREATE UNIQUE INDEX [IX_Name] ON [dbo].[Groups]([Name])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201504271308420_GroupHasUniqueNameAndSecret', N'DataAccessLayer.Migrations.Configuration',  0x1F8B0800000000000400D51DD96EE4B8F13D40FE41E8A724F076FBC80C26467B175E1F1B23E303D39E45DE0C5A62B785D1D12B51B336827C591EF249F98590BA5ABC491D2D19030CA6A562B158AC2A56955835FFFBCF7F973FBD8681F31D26A91F4767B3A3F9E1CC81911B7B7EB4399B6568FDC3A7D94F3FFEF10FCB2B2F7C757EADE04E081C1E19A567B31784B6A78B45EABEC010A4F3D07793388DD768EEC6E10278F1E2F8F0F06F8BA3A305C428661897E32CBF6411F24398FFC03F2FE2C8855B9481E036F6609096CFF19B558ED5B903214CB7C08567B34B80C0B9EBC234FD0CDE603273CE031F6032563058CF1C10453102081379FA35852B94C4D166B5C50F40F0F8B685186E0D821496C49FEEC04DD771784CD6B1D80DAC50B9598AE2D012E1D149C998053BBC157B6735E330EBAE308BD11B5975CEBEB3D92F499C6D670E3BD3E9459010288EB5F31C830FD3F9D5EB1646294C2FF1383F9AE7880E1C06FCA0160F2C45E4CF81739105284BE05904339480E0C079C89E03DFFD077C7B8CBFC1E82CCA82A04933A61ABFA31EE0470F49BC85097AFB02D7CD95DC783367410F5EB0A3EBB1ECC062C137113A399E3977980CF01CC05A381ACC59A13881BFC008260041EF01200413BCB7371ECCD9CB91C04C48FEAE66C3D288B56AE6DC82D7CF30DAA017AC6F87588FAEFD57E8554F4A0ABE463E56423C0825191450A89E7505DD0422C5BCF89FBDCC7B07BEFB9B9C510C05B7307CC6D23073BEC0207F9FBEF8DB4249E7E7DB2D1682FC29D6D1E429DF140C7A9DC4E197984C2686787A04C9265F56AC045BC559E232B42E173B75502A0983B395BA3038C6D494364AB2D30F91C41E7F1A4262AFB0610914D31E7FF86834ADC12CF8B059FB4908EB55FE1C6369029135CD0F204D7F8F13EFEF207DE941D7B42A9D255854570884DBC1677B7889237897111DDEE75CBD6DCDE3EFF13570B1E9BE8AC8A8CEF83EC7EEB7384357918755197E456E8590FC7CF4437304BD9053D8926B2CCCD0BB88B12FA53BD0D4E88891D21C54A6FAD7FAC0B808801F9A9D1715A8FCBC2820EA8340765E9460D5B1624A6A755EF579B4E948654E4053523FC71B3F3223B50295935A4068492DC16C4925C8CC282D21E584E6005A3A0B285B321F1310A5D8B4101285D436009E1E40827CD7DF82085104CB60380F470AD8C9C7A99C57C2865C07545ECE6D1D709CA7DB3B88E6D5E8C2DF79BB4E304E7C0C7E9B73680F1CE3C13B97E8D8D4253A397A5E9F7CFAF01178271FFF0A4F3EECDF3D1A3E7C20AC1CC31DCBB78F4C3AF8E99FCFF42B08B2BEA76AA50DB9EDEA5F1B72B4D3D7869C4CFCF8BBEF11BF6FA11F510163F446F0953CDBEA1C43D9BED5815AE6BE27DF8F0D68A52EE490EA5F5B08D6E92B8B589485A064416DA47E2CEB5FD13B11896B38417D642F1BE8C6CCCC34C868231BCCF07D3924973075137F5BECC5E0E94536EC0D9BB1EE658C37C21E89610A586F931FC01B5EB7524F5ABA48EAC8531B759450E270237FC9C5463C846D5C44873A7DC7452A7A850154A7A3ADFF63ED7D1C69EF25593C76AE8A68BF58C89BFBFD5482EDC49B7FCB09B600C446A4CFD33476FD9C2AF1F78C2A7946AFF42AF21CB34C5AC173415606EF0096509F3CC374E1AD9FB1F2761F5DC20022E89CBBC5F7D10B90BAC0E3F98ED7E659D257E74D76F409921C34857FE126C69A0013320A908F0429D6333F42BCDAF811B1368111C798D1866A479850CFC3BEB984D89D218B336289090152577651CFC5EC908E53CB454312ED04B44C999A0A009B3F3516507EFBB59308A4AC3C6B95A83B30A34CCA9AD2C96668A7A6AD4C6A58A2AD6512662FDA4A736C046DA559F2EEB4B5C8C59BEE3F93989F9A78D25F0424D25938877B114E8A5D23C826C58FC98B261F02C9F65D110FEDF69CCA3A981F1DF2484A776AF42AE4ADE44F4ABBC9DE4BEE645909A074633A12B067E9A3C25B134111C7BABDCAA2F833635B6FC9823182904846AF2A3EE28DB1C8100F745028A2B2FD9F12722EEDE1889073C2647271227C100D2DE2633C06E11130E1DD97CB67F212BE2241A6072FAC4CF6A465B280950C827C05117323641794D3869ED3507A789928E046733AA9C1C385BC22A482B8D8026D754B4489B674E02DD096573A94580B95D220A5AF6770F82843AA41A5A349484F431A77986417981AC0EAAB4EACBA18E76BEA35D542C6A99E716EA5814B2468AC39A539D1824B9552E9B9244A19D8240D3A7189C90D3470550BE89D339512EA3923CA1FD864103A718689F625F2532DA6772E95FAAB67922086B588623BB1888E38251C2A17D29941826F443C73345194611CD558096D9115BC91474E43E894FC73949A2772DFDECEBBEFCA21A13FAF93C5167C127DD1E039A473F24DDDFCC60284526FEA95F7A449D56796DA8DACDF2D1745655AF960B99094B02D6FC176EB479B46495BF9C45915F56C173FACEC6BBDC202C7C2A5F8CC3ABDF54C284EC006326FF1D498D26B3F4911B905F10CC827B00B2FE4C0844EB3C477AAA6A4FC627E0B2B97AA0227FF2E2F1830B738843668C7C56BBCB0908427F9BD06D650F0E31C5252080290C86E055CC44116469A8C821C4FF169B289A478628EA1AADD6AE2A89EF158960B86115C64C5319B0B7BE9AD33DA58998A9BEF2B1BDBD8EFB016838CC3EC36DBED7059A9D444503EB2C4D12876E19035DE9963A5EB919A38E93756B2D82C3A6244B2F9CA82CA6669114564F3452B7C128E8A21CC67E08B899AD8F9B7E6980565454DD482D72D700B6866DF996315541E35110B5E9BE3DE95213551EE9E4EC6FA09C2DF2EA690CFC8D81B43031CC398C3EA930FBB6576581AF5064D448DC796B8CA8A020E59F97C92A2240DD6DB89529185EB264A121C729B435DD3A74D8EB2B6408E93BA7B4F997555ED811C9F9DC04E402C64E989765291672DBB098518C5B006A2FA72D0C422FB9A30DAAE51817C970D6BA6A9EDF74A395AEAE4D0F7C9290F4775535D8593BA2CDEC448BDB0703E4281C711DABA197D857AF41D7056BC776F26239EFD19928E46C4CE8074F34FCC03F2A17785CE32D15B23FBF6611C3FD7233451B234A5CBAA9AF28B09CF3BA39D63918A3F053788694DA7F49BB4A54578525C2A91D1C9E613AD85419E9A363A3198318AD341934111B05B97D63664B8D961F3A439782C4545973D1F45A8F5C2C2E5A05990DA6ED5B96826E7BC2CF3BFFADE6A5C42B800993995AF7D365BBDA508867302305FFD165C043E24C76F05700B227F0D535414A5CC8E0F8F8E990E6DD3E996B648532F10E4CFB99669F48EEDB543994F98ABADD9EB503F137D0789FB02448DAE6E220FBE9ECDFEE5DCA45F23FFB70C9E3A8F49069D7FF32DCC3AF527AB48F853085EFFDC57D1A8D2B6EDA31C6AC758B620AA4B6B2C21D6BCE06987B57D27AC671FF5D2054BBAA3AD3B5D75C228E866D517BE5E5828EB56D50697B45395877FA2BC5395DD62C59DABDA9026ED5A951BB98E3DAB4CEC18813F75AA91F9E303B5716334ABAFBE09820CED3E4CD2208709DDA4C07C1B6EFEF9540C3D70EE13EC399C3A87ECC9D2C258728D6B3A293ADF9CC6025D8706342D24E39DF56EE9ED7414B466E90DF798A2DDBE1F4B0BE1D97B2F93B1CD04DDE1C48A9A62E83E76569AC71DAFC1C8202788A0A7884D48207034A83E21EB2006F6FE85280E33908E72182D1E3D341719F2DC696B3526121FB5F302C9A247F002F5E95833DE1B659D246909595253BF7BA2595B98501E4DAFC69D59640B056EA1C66DAC7A0F390A9B74691B29B23A27580AEC382F45D4C9984E5164F7D5BD650225F6826AB3F17BB4ECB3505275A96B6205F416DFFC262D73C233564196490F97772479265F7CDBFB14230AA2E83B5D63DBF97D1EBECDC23485CE7CCBFB68DAD05ADEB45FD8471035BB864B13307682BAEBF1DB2AEDFB80955D759DF8016BD53C6962B25654BF8FDE2369DF9226B9F9363141336F8534EEA149B56368DBEFE61D1C91EAFBBCEFA595912E1268B19BC3CB95CA8AAAC006913A5B39D05E806B23816D2CA8D5ADBC31455413234CEF4C9DAAA88E70CE7694D1C984AF2D3B9E4DA5C9D998DE9DA6C0601AAEDDE85DCCF8BE0DEC9ED29DAC4A976E9774A65DBDE292EFD9CC7B8EF19E17596B499F151633678BB8393808D16C453B7C71CB12764A41CE989B5400A39E56D2C44A3577194E2BE72E61D4734B1A20A9E62E3450397501A29E59DC0E859D9872DFB839A9B7A2E954DD6DA48BD42C50B738B3858933723A0956288D08D28ABF7A7552016BB8CFC0F3848DDCBB8EEADAC3BDE38CADF21B93A0E8ACF1963D147B6A5557773132ED5827A99A1980452A519797890DC6A8AA4D9331A3C4253A7C1EA3B122E11226C994E1DAF9F5A252D44127E910D03F5386EADED70B4B9AC793B83CBE3B437AEFD6D7464504673F5762DEEF5285CDF2CC5BF2E94D6A8B251907EC66659603324C6E582DAB1F0750A029306EE06E86429B20B2077BB12C16DD0BF90A511CB46611A9172A7E5DC2D4DFEC502C31CE08BA54B85AC3DC44EBB88A9A198A2A10E692D62D44C02315CD582ED778EFF16B522A94FFC75E79F90529587B86DE4D749FA16D86F09261F81C50750B24FA56CD9FB768A4695EDEE7978ED33E9680C9F44989D57DF473E6075E4DF7B5E0FA97040509EBCB6BD5642F11B95EBD79AB31DDC59121A2927D7536E21186DB00234BEFA315F80EDBD086C5EF33DC00F76D57C82143A2DF089AEDCB4B1F6C1210A6258EDD78FC13CBB017BEFEF87F0B2433D2AA8A0000 , N'6.1.3-40302')

™'
PC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Damdfe.Fast\DamdfeFrMDFe.cs
	namespace(( 	
MDFe((
 
.(( 
Damdfe(( 
.(( 
Fast(( 
{)) 
public** 

class** 
DamdfeFrMDFe** 
{++ 
	protected,, 
Report,, 
	Relatorio,, "
;,," #
public.. 
DamdfeFrMDFe.. 
(.. 
MDFeProcMDFe.. (
proc..) -
,..- .
ConfiguracaoDamdfe../ A
config..B H
)..H I
{// 	
	Relatorio00 
=00 
new00 
Report00 "
(00" #
)00# $
;00$ %
RegisterData11 
(11 
proc11 
)11 
;11 

Configurar22 
(22 
config22 
)22 
;22 
}44 	
public66 
DamdfeFrMDFe66 
(66 
)66 
{77 	
	Relatorio88 
=88 
new88 
Report88 "
(88" #
)88# $
;88$ %
}99 	
public;; 
void;; 
RegisterData;;  
(;;  !
MDFeProcMDFe;;! -
proc;;. 2
);;2 3
{<< 	
	Relatorio== 
.== 
RegisterData== "
(==" #
new==# &
[==& '
]==' (
{==) *
proc==+ /
}==0 1
,==1 2
$str==3 A
,==A B
$num==C E
)==E F
;==F G
	Relatorio>> 
.>> 
GetDataSource>> #
(>># $
$str>>$ 2
)>>2 3
.>>3 4
Enabled>>4 ;
=>>< =
true>>> B
;>>B C
	Relatorio?? 
.?? 
Load?? 
(?? 
new?? 
MemoryStream?? +
(??+ ,

Properties??, 6
.??6 7
	Resources??7 @
.??@ A
MDFeRetrato??A L
)??L M
)??M N
;??N O
}@@ 	
publicBB 
voidBB 

ConfigurarBB 
(BB 
ConfiguracaoDamdfeBB 1
configBB2 8
)BB8 9
{CC 	
	RelatorioDD 
.DD 
SetParameterValueDD '
(DD' (
$strDD( =
,DD= >
configDD? E
.DDE F
DocumentoCanceladoDDF X
)DDX Y
;DDY Z
	RelatorioEE 
.EE 
SetParameterValueEE '
(EE' (
$strEE( <
,EE< =
configEE> D
.EED E
DocumentoEncerradoEEE W
)EEW X
;EEX Y
	RelatorioFF 
.FF 
SetParameterValueFF '
(FF' (
$strFF( 7
,FF7 8
configFF9 ?
.FF? @
DesenvolvedorFF@ M
)FFM N
;FFN O
(GG 
(GG 
PictureObjectGG 
)GG 
	RelatorioGG %
.GG% &

FindObjectGG& 0
(GG0 1
$strGG1 =
)GG= >
)GG> ?
.GG? @
ImageGG@ E
=GGF G
configGGH N
.GGN O
	ObterLogoGGO X
(GGX Y
)GGY Z
;GGZ [
}HH 	
publicNN 
voidNN 

VisualizarNN 
(NN 
boolNN #
modalNN$ )
=NN* +
trueNN, 0
)NN0 1
{OO 	
	RelatorioPP 
.PP 
ShowPP 
(PP 
modalPP  
)PP  !
;PP! "
}QQ 	
publicXX 
voidXX 
ExibirDesignXX  
(XX  !
boolXX! %
modalXX& +
=XX, -
falseXX. 3
)XX3 4
{YY 	
	RelatorioZZ 
.ZZ 
DesignZZ 
(ZZ 
modalZZ "
)ZZ" #
;ZZ# $
}[[ 	
publicbb 
voidbb 
Imprimirbb 
(bb 
boolbb !
exibirDialogobb" /
=bb0 1
truebb2 6
,bb6 7
stringbb8 >

impressorabb? I
=bbJ K
$strbbL N
)bbN O
{cc 	
	Relatoriodd 
.dd 
PrintSettingsdd #
.dd# $

ShowDialogdd$ .
=dd/ 0
exibirDialogodd1 >
;dd> ?
	Relatorioee 
.ee 
PrintSettingsee #
.ee# $
Printeree$ +
=ee, -

impressoraee. 8
;ee8 9
	Relatorioff 
.ff 
Printff 
(ff 
)ff 
;ff 
}gg 	
publicmm 
voidmm 
ExportarPdfmm 
(mm  
stringmm  &
arquivomm' .
)mm. /
{nn 	
	Relatoriooo 
.oo 
Prepareoo 
(oo 
)oo 
;oo  
	Relatoriopp 
.pp 
Exportpp 
(pp 
newpp  
	PDFExportpp! *
(pp* +
)pp+ ,
,pp, -
arquivopp. 5
)pp5 6
;pp6 7
}qq 	
}rr 
}ss †
[C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Damdfe.Fast\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str +
)+ ,
], -
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[		 
assembly		 	
:			 
!
AssemblyConfiguration		  
(		  !
$str		! #
)		# $
]		$ %
[

 
assembly

 	
:

	 

AssemblyCompany

 
(

 
$str

 
)

 
]

 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str -
)- .
]. /
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *
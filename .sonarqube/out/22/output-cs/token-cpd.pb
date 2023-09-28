µ
JC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Fast\DanfeBase.cs
	namespace&& 	
NFe&&
 
.&& 
Danfe&& 
.&& 
Fast&& 
{'' 
public(( 

class(( 
	DanfeBase(( 
:(( 
IDanfe(( #
{)) 
	protected** 
Report** 
	Relatorio** "
;**" #
public00 
void00 

Visualizar00 
(00 
bool00 #
modal00$ )
=00* +
true00, 0
)000 1
{11 	
	Relatorio22 
.22 
Show22 
(22 
modal22  
)22  !
;22! "
}33 	
public:: 
void:: 
ExibirDesign::  
(::  !
bool::! %
modal::& +
=::, -
false::. 3
)::3 4
{;; 	
	Relatorio<< 
.<< 
Design<< 
(<< 
modal<< "
)<<" #
;<<# $
}== 	
publicDD 
voidDD 
ImprimirDD 
(DD 
boolDD !
exibirDialogoDD" /
=DD0 1
trueDD2 6
,DD6 7
stringDD8 >

impressoraDD? I
=DDJ K
$strDDL N
)DDN O
{EE 	
	RelatorioFF 
.FF 
PrintSettingsFF #
.FF# $

ShowDialogFF$ .
=FF/ 0
exibirDialogoFF1 >
;FF> ?
	RelatorioGG 
.GG 
PrintSettingsGG #
.GG# $
PrinterGG$ +
=GG, -

impressoraGG. 8
;GG8 9
	RelatorioHH 
.HH 
PrintHH 
(HH 
)HH 
;HH 
}II 	
publicOO 
voidOO 
ExportarPdfOO 
(OO  
stringOO  &
arquivoOO' .
)OO. /
{PP 	
	RelatorioQQ 
.QQ 
PrepareQQ 
(QQ 
)QQ 
;QQ  
	RelatorioRR 
.RR 
ExportRR 
(RR 
newRR  
	PDFExportRR! *
(RR* +
)RR+ ,
,RR, -
arquivoRR. 5
)RR5 6
;RR6 7
}SS 	
}TT 
}UU ù8
QC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Fast\NFCe\DanfeFrNfce.cs
	namespace++ 	
NFe++
 
.++ 
Danfe++ 
.++ 
Fast++ 
.++ 
NFCe++ 
{,, 
public00 

class00 
DanfeFrNfce00 
:00 
	DanfeBase00 (
{11 
public99 
DanfeFrNfce99 
(99 
nfeProc99 "
proc99# '
,99' (!
ConfiguracaoDanfeNfce99) >!
configuracaoDanfeNfce99? T
,99T U
string99V \
cIdToken99] e
,99e f
string99g m
csc99n q
)99q r
{:: 	
	Relatorio== 
=== 
new== 
Report== "
(==" #
)==# $
;==$ %
	Relatorio>> 
.>> 
RegisterData>> "
(>>" #
new>># &
[>>& '
]>>' (
{>>) *
proc>>+ /
}>>0 1
,>>1 2
$str>>3 9
,>>9 :
$num>>; =
)>>= >
;>>> ?
	Relatorio?? 
.?? 
GetDataSource?? #
(??# $
$str??$ *
)??* +
.??+ ,
Enabled??, 3
=??4 5
true??6 :
;??: ;
	Relatorio@@ 
.@@ 
Load@@ 
(@@ 
new@@ 
MemoryStream@@ +
(@@+ ,

Properties@@, 6
.@@6 7
	Resources@@7 @
.@@@ A
NFCe@@A E
)@@E F
)@@F G
;@@G H
	RelatorioAA 
.AA 
SetParameterValueAA '
(AA' (
$strAA( @
,AA@ A!
configuracaoDanfeNfceAAB W
.AAW X
DetalheVendaNormalAAX j
)AAj k
;AAk l
	RelatorioBB 
.BB 
SetParameterValueBB '
(BB' (
$strBB( E
,BBE F!
configuracaoDanfeNfceBBG \
.BB\ ]#
DetalheVendaContigenciaBB] t
)BBt u
;BBu v
	RelatorioCC 
.CC 
SetParameterValueCC '
(CC' (
$strCC( A
,CCA B!
configuracaoDanfeNfceCCC X
.CCX Y
ImprimeDescontoItemCCY l
)CCl m
;CCm n
	RelatorioDD 
.DD 
SetParameterValueDD '
(DD' (
$strDD( ;
,DD; <!
configuracaoDanfeNfceDD= R
.DDR S
ModoImpressaoDDS `
)DD` a
;DDa b
	RelatorioEE 
.EE 
SetParameterValueEE '
(EE' (
$strEE( 7
,EE7 8!
configuracaoDanfeNfceEE9 N
.EEN O
DocumentoCanceladoEEO a
)EEa b
;EEb c
(FF 
(FF 

ReportPageFF 
)FF 
	RelatorioFF "
.FF" #

FindObjectFF# -
(FF- .
$strFF. 6
)FF6 7
)FF7 8
.FF8 9

LeftMarginFF9 C
=FFD E!
configuracaoDanfeNfceGG %
.GG% &
MargemEsquerdaGG& 4
;GG4 5
(HH 
(HH 

ReportPageHH 
)HH 
	RelatorioHH "
.HH" #

FindObjectHH# -
(HH- .
$strHH. 6
)HH6 7
)HH7 8
.HH8 9
RightMarginHH9 D
=HHE F!
configuracaoDanfeNfceHHG \
.HH\ ]
MargemDireitaHH] j
;HHj k
(II 
(II 
PictureObjectII 
)II 
	RelatorioII %
.II% &

FindObjectII& 0
(II0 1
$strII1 =
)II= >
)II> ?
.II? @
ImageII@ E
=IIF G!
configuracaoDanfeNfceIIH ]
.II] ^
	ObterLogoII^ g
(IIg h
)IIh i
;IIi j
(JJ 
(JJ 

TextObjectJJ 
)JJ 
	RelatorioJJ "
.JJ" #

FindObjectJJ# -
(JJ- .
$strJJ. 6
)JJ6 7
)JJ7 8
.JJ8 9
TextJJ9 =
=JJ> ?
procJJ@ D
.JJD E
NFeJJE H
.JJH I

infNFeSuplJJI S
.JJS T
ObterUrlJJT \
(JJ\ ]
procJJ] a
.JJa b
NFeJJb e
.JJe f
infNFeJJf l
.JJl m
ideJJm p
.JJp q
tpAmbJJq v
,JJv w
procJJx |
.JJ| }
NFe	JJ} Ä
.
JJÄ Å
infNFe
JJÅ á
.
JJá à
ide
JJà ã
.
JJã å
cUF
JJå è
,
JJè ê$
TipoUrlConsultaPublica
JJë ß
.
JJß ®
UrlConsulta
JJ® ≥
)
JJ≥ ¥
;
JJ¥ µ
(KK 
(KK 
BarcodeObjectKK 
)KK 
	RelatorioKK %
.KK% &

FindObjectKK& 0
(KK0 1
$strKK1 <
)KK< =
)KK= >
.KK> ?
TextKK? C
=KKD E
procKKF J
.KKJ K
NFeKKK N
.KKN O

infNFeSuplKKO Y
==KKZ \
nullKK] a
?KKb c
procKKd h
.KKh i
NFeKKi l
.KKl m

infNFeSuplKKm w
.KKw x
ObterUrlQrCode	KKx Ü
(
KKÜ á
proc
KKá ã
.
KKã å
NFe
KKå è
,
KKè ê
cIdToken
KKë ô
,
KKô ö
csc
KKõ û
)
KKû ü
:
KK† °
proc
KK¢ ¶
.
KK¶ ß
NFe
KKß ™
.
KK™ ´

infNFeSupl
KK´ µ
.
KKµ ∂
qrCode
KK∂ º
;
KKº Ω
	RelatorioNN 
.NN 
PrintSettingsNN #
.NN# $
CopiesNN$ *
=NN+ ,
(NN- .
procNN. 2
.NN2 3
NFeNN3 6
.NN6 7
infNFeNN7 =
.NN= >
ideNN> A
.NNA B
tpEmisNNB H
==NNI K
TipoEmissaoNNL W
.NNW X
teNormalNNX `
|NNa b
(NNc d
procNNd h
.NNh i
protNFeNNi p
!=NNq s
nullNNt x
&&NNy {
proc	NN| Ä
.
NNÄ Å
protNFe
NNÅ à
.
NNà â
infProt
NNâ ê
!=
NNë ì
null
NNî ò
&&
NNô õ
NfeSituacao
NNú ß
.
NNß ®

Autorizada
NN® ≤
(
NN≤ ≥
proc
NN≥ ∑
.
NN∑ ∏
protNFe
NN∏ ø
.
NNø ¿
infProt
NN¿ «
.
NN« »
cStat
NN» Õ
)
NNÕ Œ
)
NNŒ œ
)OOe f
?OOg h
$numOOi j
:OOk l
$numOOm n
;OOn o
}RR 	
public\\ 
DanfeFrNfce\\ 
(\\ 
Classes\\ "
.\\" #
NFe\\# &
nfe\\' *
,\\* +!
ConfiguracaoDanfeNfce\\, A!
configuracaoDanfeNfce\\B W
,\\W X
string\\Y _
cIdToken\\` h
,\\h i
string\\j p
csc\\q t
)\\t u
:\\v w
this\\x |
(\\| }
new	\\} Ä
nfeProc
\\Å à
(
\\à â
)
\\â ä
{
\\ã å
NFe
\\ç ê
=
\\ë í
nfe
\\ì ñ
}
\\ó ò
,
\\ò ô#
configuracaoDanfeNfce
\\ö Ø
,
\\Ø ∞
cIdToken
\\± π
,
\\π ∫
csc
\\ª æ
)
\\æ ø
{]] 	
}^^ 	
}__ 
}`` Ë
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Fast\NFe\DanfeFrEvento.cs
	namespace'' 	
NFe''
 
.'' 
Danfe'' 
.'' 
Fast'' 
.'' 
NFe'' 
{(( 
public,, 

class,, 
DanfeFrEvento,, 
:,,  
	DanfeBase,,! *
{-- 
public55 
DanfeFrEvento55 
(55 
nfeProc55 $
proc55% )
,55) *
Classes55+ 2
.552 3
Servicos553 ;
.55; <
Consulta55< D
.55D E
procEventoNFe55E R
procEventoNFe55S `
,55` a 
ConfiguracaoDanfeNfe55b v!
configuracaoDanfeNfe	55w ã
,
55ã å
string
55ç ì
desenvolvedor
55î °
=
55¢ £
$str
55§ ¶
)
55¶ ß
{66 	
	Relatorio99 
=99 
new99 
Report99 "
(99" #
)99# $
;99$ %
	Relatorio:: 
.:: 
Load:: 
(:: 
new:: 
MemoryStream:: +
(::+ ,

Properties::, 6
.::6 7
	Resources::7 @
.::@ A
	NFeEvento::A J
)::J K
)::K L
;::L M
	Relatorio;; 
.;; 
RegisterData;; "
(;;" #
new;;# &
[;;& '
];;' (
{;;) *
proc;;+ /
};;0 1
,;;1 2
$str;;3 8
,;;8 9
$num;;: <
);;< =
;;;= >
	Relatorio<< 
.<< 
RegisterData<< "
(<<" #
new<<# &
[<<& '
]<<' (
{<<) *
procEventoNFe<<+ 8
}<<9 :
,<<: ;
$str<<< K
,<<K L
$num<<M O
)<<O P
;<<P Q
	Relatorio== 
.== 
GetDataSource== #
(==# $
$str==$ )
)==) *
.==* +
Enabled==+ 2
===3 4
true==5 9
;==9 :
	Relatorio>> 
.>> 
GetDataSource>> #
(>># $
$str>>$ 3
)>>3 4
.>>4 5
Enabled>>5 <
=>>= >
true>>? C
;>>C D
	Relatorio?? 
.?? 
SetParameterValue?? '
(??' (
$str??( 7
,??7 8
desenvolvedor??9 F
)??F G
;??G H
}BB 	
}CC 
}DD 
OC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Fast\NFe\DanfeFrNfe.cs
	namespace'' 	
NFe''
 
.'' 
Danfe'' 
.'' 
Fast'' 
.'' 
NFe'' 
{(( 
public,, 

class,, 

DanfeFrNfe,, 
:,, 
	DanfeBase,, '
{-- 
public44 

DanfeFrNfe44 
(44 
nfeProc44 !
proc44" &
,44& ' 
ConfiguracaoDanfeNfe44( < 
configuracaoDanfeNfe44= Q
,44Q R
string44S Y
desenvolvedor44Z g
=44h i
$str44j l
)44l m
{55 	
	Relatorio88 
=88 
new88 
Report88 "
(88" #
)88# $
;88$ %
	Relatorio99 
.99 
RegisterData99 "
(99" #
new99# &
[99& '
]99' (
{99) *
proc99+ /
}990 1
,991 2
$str993 8
,998 9
$num99: <
)99< =
;99= >
	Relatorio:: 
.:: 
GetDataSource:: #
(::# $
$str::$ )
)::) *
.::* +
Enabled::+ 2
=::3 4
true::5 9
;::9 :
	Relatorio;; 
.;; 
Load;; 
(;; 
new;; 
MemoryStream;; +
(;;+ ,

Properties;;, 6
.;;6 7
	Resources;;7 @
.;;@ A

NFeRetrato;;A K
);;K L
);;L M
;;;M N
	Relatorio<< 
.<< 
SetParameterValue<< '
(<<' (
$str<<( 4
,<<4 5 
configuracaoDanfeNfe<<6 J
.<<J K

DuasLinhas<<K U
)<<U V
;<<V W
	Relatorio== 
.== 
SetParameterValue== '
(==' (
$str==( 3
,==3 4 
configuracaoDanfeNfe==5 I
.==I J
DocumentoCancelado==J \
)==\ ]
;==] ^
	Relatorio>> 
.>> 
SetParameterValue>> '
(>>' (
$str>>( 7
,>>7 8
desenvolvedor>>9 F
)>>F G
;>>G H
(?? 
(?? 
PictureObject?? 
)?? 
	Relatorio?? %
.??% &

FindObject??& 0
(??0 1
$str??1 =
)??= >
)??> ?
.??? @
Image??@ E
=??F G 
configuracaoDanfeNfe??H \
.??\ ]
	ObterLogo??] f
(??f g
)??g h
;??h i
}BB 	
publicKK 

DanfeFrNfeKK 
(KK 
ClassesKK !
.KK! "
NFeKK" %
nfeKK& )
,KK) * 
ConfiguracaoDanfeNfeKK+ ? 
configuracaoDanfeNfeKK@ T
,KKT U
stringKKV \
desenvolvedorKK] j
)KKj k
:KKl m
thisKKn r
(KKr s
newKKs v
nfeProcKKw ~
(KK~ 
)	KK Ä
{
KKÅ Ç
NFe
KKÉ Ü
=
KKá à
nfe
KKâ å
}
KKç é
,
KKé è"
configuracaoDanfeNfe
KKê §
,
KK§ •
desenvolvedor
KK¶ ≥
)
KK≥ ¥
{LL 	
}MM 	
}NN 
}OO É
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Fast\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str )
)) *
]* +
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
$str +
)+ ,
], -
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
$str"" &
)""& '
]""' (
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## *
)##* +
]##+ ,
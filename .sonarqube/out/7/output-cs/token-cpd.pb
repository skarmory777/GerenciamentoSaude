�
PC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\AdmCsc\ExtAdmCscNFCe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
AdmCsc%% 
{&& 
public'' 

static'' 
class'' 
ExtAdmCscNFCe'' %
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0

admCscNFCe..1 ;

admCscNFCe..< F
)..F G
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2

admCscNFCe002 <
)00< =
;00= >
}11 	
}22 
}33 �	
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\AdmCsc\ExtretAdmCscNFCe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
AdmCsc%% 
{&& 
public'' 

static'' 
class'' 
ExtretAdmCscNFCe'' (
{(( 
public// 
static// 
retAdmCscNFCe// #
CarregarDeXmlString//$ 7
(//7 8
this//8 <
retAdmCscNFCe//= J
retAdmCscNFCe//K X
,//X Y
string//Z `
	xmlString//a j
)//j k
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retAdmCscNFCe112 ?
>11? @
(11@ A
	xmlString11A J
)11J K
;11K L
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retAdmCscNFCe991 >
retDownloadNFe99? M
)99M N
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retDownloadNFe;;2 @
);;@ A
;;;A B
}<< 	
}== 
}>> �$
PC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Assinatura\Assinador.cs
	namespace)) 	
NFe))
 
.)) 
Utils)) 
.)) 

Assinatura)) 
{** 
public++ 

static++ 
class++ 
	Assinador++ !
{,, 
public55 
static55 
	Signature55 
ObterAssinatura55  /
<55/ 0
T550 1
>551 2
(552 3
T553 4
objeto555 ;
,55; <
string55= C
id55D F
,55F G
X509Certificate255H X
certificadoDigital55Y k
=55l m
null55n r
)55r s
where55t y
T55z {
:55| }
class	55~ �
{66 	
var77 
objetoLocal77 
=77 
objeto77 $
;77$ %
if88 
(88 
id88 
==88 
null88 
)88 
throw99 
new99 
	Exception99 #
(99# $
$str99$ d
)99d e
;99e f
var;; 
certificado;; 
=;; 
certificadoDigital;; 0
??;;1 3
CertificadoDigital;;4 F
.;;F G
ObterCertificado;;G W
(;;W X
ConfiguracaoServico;;X k
.;;k l
	Instancia;;l u
.;;u v
Certificado	;;v �
)
;;� �
;
;;� �
try<< 
{== 
var>> 
	documento>> 
=>> 
new>>  #
XmlDocument>>$ /
{>>0 1
PreserveWhitespace>>2 D
=>>E F
true>>G K
}>>L M
;>>M N
	documento?? 
.?? 
LoadXml?? !
(??! "

FuncoesXml??" ,
.??, -
ClasseParaXmlString??- @
(??@ A
objetoLocal??A L
)??L M
)??M N
;??N O
var@@ 
docXml@@ 
=@@ 
new@@  
	SignedXml@@! *
(@@* +
	documento@@+ 4
)@@4 5
{@@6 7

SigningKey@@8 B
=@@C D
certificado@@E P
.@@P Q

PrivateKey@@Q [
}@@\ ]
;@@] ^
varAA 
	referenceAA 
=AA 
newAA  #
	ReferenceAA$ -
{AA. /
UriAA0 3
=AA4 5
$strAA6 9
+AA: ;
idAA< >
}AA? @
;AA@ A
varDD 
envelopedSigntatureDD '
=DD( )
newDD* -.
"XmlDsigEnvelopedSignatureTransformDD. P
(DDP Q
)DDQ R
;DDR S
	referenceEE 
.EE 
AddTransformEE &
(EE& '
envelopedSigntatureEE' :
)EE: ;
;EE; <
varGG 
c14TransformGG  
=GG! "
newGG# & 
XmlDsigC14NTransformGG' ;
(GG; <
)GG< =
;GG= >
	referenceHH 
.HH 
AddTransformHH &
(HH& '
c14TransformHH' 3
)HH3 4
;HH4 5
docXmlJJ 
.JJ 
AddReferenceJJ #
(JJ# $
	referenceJJ$ -
)JJ- .
;JJ. /
varMM 
keyInfoMM 
=MM 
newMM !
KeyInfoMM" )
(MM) *
)MM* +
;MM+ ,
keyInfoNN 
.NN 
	AddClauseNN !
(NN! "
newNN" %
KeyInfoX509DataNN& 5
(NN5 6
certificadoNN6 A
)NNA B
)NNB C
;NNC D
docXmlPP 
.PP 
KeyInfoPP 
=PP  
keyInfoPP! (
;PP( )
docXmlQQ 
.QQ 
ComputeSignatureQQ '
(QQ' (
)QQ( )
;QQ) *
varTT 
xmlDigitalSignatureTT '
=TT( )
docXmlTT* 0
.TT0 1
GetXmlTT1 7
(TT7 8
)TT8 9
;TT9 :
varUU 

assinaturaUU 
=UU  

FuncoesXmlUU! +
.UU+ ,
XmlStringParaClasseUU, ?
<UU? @
	SignatureUU@ I
>UUI J
(UUJ K
xmlDigitalSignatureUUK ^
.UU^ _
OuterXmlUU_ g
)UUg h
;UUh i
returnVV 

assinaturaVV !
;VV! "
}WW 
finallyXX 
{YY 
if\\ 
(\\ 
!\\ 
ConfiguracaoServico\\ (
.\\( )
	Instancia\\) 2
.\\2 3
Certificado\\3 >
.\\> ?
ManterDadosEmCache\\? Q
&\\R S
certificadoDigital\\T f
==\\g i
null\\j n
)\\n o
certificado]] 
.]]  
Reset]]  %
(]]% &
)]]& '
;]]' (
}^^ 
}`` 	
}aa 
}bb �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Autorizacao\ExtenviNFe3.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Autorizacao%% 
{&& 
public'' 

static'' 
class'' 
ExtenviNFe3'' #
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
enviNFe3..1 9
pedEnvio..: B
)..B C
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
pedEnvio002 :
)00: ;
;00; <
}11 	
}22 
}33 �H
EC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Conversao.cs
	namespace)) 	
NFe))
 
.)) 
Utils)) 
{** 
public++ 

static++ 
class++ 
	Conversao++ !
{,, 
public-- 
static-- 
string-- #
VersaoServicoParaString-- 4
(--4 5
this--5 9

ServicoNFe--: D

servicoNFe--E O
,--O P
VersaoServico--Q ^
?--^ _
versaoServico--` m
)--m n
{.. 	
switch// 
(// 
versaoServico// !
)//! "
{00 
case11 
VersaoServico11 "
.11" #
ve10011# (
:11( )
return22 
$str22 !
;22! "
case33 
VersaoServico33 "
.33" #
ve20033# (
:33( )
switch44 
(44 

servicoNFe44 &
)44& '
{55 
case66 

ServicoNFe66 '
.66' ( 
NfeConsultaProtocolo66( <
:66< =
return77 "
$str77# )
;77) *
}88 
return99 
$str99 !
;99! "
case:: 
VersaoServico:: "
.::" #
ve310::# (
:::( )
return;; 
$str;; !
;;;! "
}<< 
return== 
$str== 
;== 
}>> 	
public@@ 
static@@ 
string@@ 
TpAmbParaString@@ ,
(@@, -
this@@- 1
TipoAmbiente@@2 >
tpAmb@@? D
)@@D E
{AA 	
switchBB 
(BB 
tpAmbBB 
)BB 
{CC 
caseDD 
TipoAmbienteDD !
.DD! "
taHomologacaoDD" /
:DD/ 0
returnEE 
$strEE (
;EE( )
caseFF 
TipoAmbienteFF !
.FF! "

taProducaoFF" ,
:FF, -
returnGG 
$strGG %
;GG% &
defaultHH 
:HH 
throwII 
newII '
ArgumentOutOfRangeExceptionII 9
(II9 :
$strII: A
,IIA B
tpAmbIIC H
,IIH I
nullIIJ N
)IIN O
;IIO P
}JJ 
}KK 	
publicMM 
staticMM 
stringMM #
VersaoServicoParaStringMM 4
(MM4 5
thisMM5 9
VersaoServicoMM: G
versaoMMH N
)MMN O
{NN 	
switchOO 
(OO 
versaoOO 
)OO 
{PP 
caseQQ 
VersaoServicoQQ "
.QQ" #
ve100QQ# (
:QQ( )
returnRR 
$strRR !
;RR! "
caseSS 
VersaoServicoSS "
.SS" #
ve200SS# (
:SS( )
returnTT 
$strTT !
;TT! "
caseUU 
VersaoServicoUU "
.UU" #
ve310UU# (
:UU( )
returnVV 
$strVV !
;VV! "
}WW 
returnXX 
nullXX 
;XX 
}YY 	
public[[ 
static[[ 
string[[ !
TipoEmissaoParaString[[ 2
([[2 3
this[[3 7
TipoEmissao[[8 C
tipoEmissao[[D O
)[[O P
{\\ 	
var]] 
s]] 
=]] 
Enum]] 
.]] 
GetName]]  
(]]  !
typeof]]! '
(]]' (
TipoEmissao]]( 3
)]]3 4
,]]4 5
tipoEmissao]]6 A
)]]A B
;]]B C
return^^ 
s^^ 
!=^^ 
null^^ 
?^^ 
s^^  
.^^  !
	Substring^^! *
(^^* +
$num^^+ ,
)^^, -
:^^. /
$str^^0 2
;^^2 3
}__ 	
publicaa 
staticaa 
stringaa 
CrtParaStringaa *
(aa* +
thisaa+ /
CRTaa0 3
crtaa4 7
)aa7 8
{bb 	
switchcc 
(cc 
crtcc 
)cc 
{dd 
caseee 
CRTee 
.ee 
SimplesNacionalee (
:ee( )
returnff 
$strff -
;ff- .
casegg 
CRTgg 
.gg +
SimplesNacionalExcessoSublimitegg 8
:gg8 9
returnhh 
$strhh B
;hhB C
caseii 
CRTii 
.ii 
RegimeNormalii %
:ii% &
returnjj 
$strjj #
;jj# $
defaultkk 
:kk 
throwll 
newll '
ArgumentOutOfRangeExceptionll 9
(ll9 :
$strll: ?
,ll? @
crtllA D
,llD E
nullllF J
)llJ K
;llK L
}mm 
}nn 	
publicpp 
staticpp 
stringpp %
ModeloDocumentoParaStringpp 6
(pp6 7
thispp7 ;
ModeloDocumentopp< K
modeloppL R
)ppR S
{qq 	
switchrr 
(rr 
modelorr 
)rr 
{ss 
casett 
ModeloDocumentott $
.tt$ %
NFett% (
:tt( )
returnuu 
$struu !
;uu! "
casevv 
ModeloDocumentovv $
.vv$ %
NFCevv% )
:vv) *
returnww 
$strww "
;ww" #
casexx 
ModeloDocumentoxx $
.xx$ %
MDFexx% )
:xx) *
returnyy 
$stryy "
;yy" #
}zz 
return{{ 
null{{ 
;{{ 
}|| 	
public~~ 
static~~ 
string~~ 
CsticmsParaString~~ .
(~~. /
this~~/ 3
Csticms~~4 ;
csticms~~< C
)~~C D
{ 	
switch
�� 
(
�� 
csticms
�� 
)
�� 
{
�� 
case
�� 
Csticms
�� 
.
�� 
Cst00
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst10
�� "
:
��" #
case
�� 
Csticms
�� 
.
�� 
	CstPart10
�� &
:
��& '
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst20
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst30
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst40
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst41
�� "
:
��" #
case
�� 
Csticms
�� 
.
�� 
CstRep41
�� %
:
��% &
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst50
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst51
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst60
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst70
�� "
:
��" #
return
�� 
$str
�� 
;
��  
case
�� 
Csticms
�� 
.
�� 
Cst90
�� "
:
��" #
case
�� 
Csticms
�� 
.
�� 
	CstPart90
�� &
:
��& '
return
�� 
$str
�� 
;
��  
default
�� 
:
�� 
throw
�� 
new
�� )
ArgumentOutOfRangeException
�� 9
(
��9 :
$str
��: C
,
��C D
csticms
��E L
,
��L M
null
��N R
)
��R S
;
��S T
}
�� 
}
�� 	
public
�� 
static
�� 
string
�� !
CsosnicmsParaString
�� 0
(
��0 1
this
��1 5
	Csosnicms
��6 ?
	csosnicms
��@ I
)
��I J
{
�� 	
return
�� 
(
�� 
(
�� 
int
�� 
)
�� 
	csosnicms
�� "
)
��" #
.
��# $
ToString
��$ ,
(
��, -
)
��- .
;
��. /
}
�� 	
public
�� 
static
�� 
string
�� (
OrigemMercadoriaParaString
�� 7
(
��7 8
this
��8 <
OrigemMercadoria
��= M
origemMercadoria
��N ^
)
��^ _
{
�� 	
return
�� 
(
�� 
(
�� 
int
�� 
)
�� 
origemMercadoria
�� )
)
��) *
.
��* +
ToString
��+ 3
(
��3 4
)
��4 5
;
��5 6
}
�� 	
}
�� 
}�� �
FC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Compressao.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
{&& 
public'' 

static'' 
class'' 

Compressao'' "
{(( 
private)) 
static)) 
void)) 

CopiarPara)) &
())& '
Stream))' -
src)). 1
,))1 2
Stream))3 9
dest)): >
)))> ?
{** 	
var++ 
bytes++ 
=++ 
new++ 
byte++  
[++  !
$num++! %
]++% &
;++& '
int-- 
cnt-- 
;-- 
while// 
(// 
(// 
cnt// 
=// 
src// 
.// 
Read// "
(//" #
bytes//# (
,//( )
$num//* +
,//+ ,
bytes//- 2
.//2 3
Length//3 9
)//9 :
)//: ;
!=//< >
$num//? @
)//@ A
{00 
dest11 
.11 
Write11 
(11 
bytes11  
,11  !
$num11" #
,11# $
cnt11% (
)11( )
;11) *
}22 
}33 	
public:: 
static:: 
byte:: 
[:: 
]:: 
Zip::  
(::  !
string::! '
str::( +
)::+ ,
{;; 	
var<< 
bytes<< 
=<< 
Encoding<<  
.<<  !
UTF8<<! %
.<<% &
GetBytes<<& .
(<<. /
str<</ 2
)<<2 3
;<<3 4
using>> 
(>> 
var>> 
msi>> 
=>> 
new>>  
MemoryStream>>! -
(>>- .
bytes>>. 3
)>>3 4
)>>4 5
using?? 
(?? 
var?? 
mso?? 
=?? 
new??  
MemoryStream??! -
(??- .
)??. /
)??/ 0
{@@ 
usingAA 
(AA 
varAA 
gsAA 
=AA 
newAA  #

GZipStreamAA$ .
(AA. /
msoAA/ 2
,AA2 3
CompressionModeAA4 C
.AAC D
CompressAAD L
)AAL M
)AAM N
{BB 

CopiarParaCC 
(CC 
msiCC "
,CC" #
gsCC$ &
)CC& '
;CC' (
}DD 
returnFF 
msoFF 
.FF 
ToArrayFF "
(FF" #
)FF# $
;FF$ %
}GG 
}HH 	
publicOO 
staticOO 
stringOO 
UnzipOO "
(OO" #
byteOO# '
[OO' (
]OO( )
bytesOO* /
)OO/ 0
{PP 	
usingQQ 
(QQ 
varQQ 
msiQQ 
=QQ 
newQQ  
MemoryStreamQQ! -
(QQ- .
bytesQQ. 3
)QQ3 4
)QQ4 5
usingRR 
(RR 
varRR 
msoRR 
=RR 
newRR  
MemoryStreamRR! -
(RR- .
)RR. /
)RR/ 0
{SS 
usingTT 
(TT 
varTT 
gsTT 
=TT 
newTT  #

GZipStreamTT$ .
(TT. /
msiTT/ 2
,TT2 3
CompressionModeTT4 C
.TTC D

DecompressTTD N
)TTN O
)TTO P
{UU 

CopiarParaVV 
(VV 
gsVV !
,VV! "
msoVV# &
)VV& '
;VV' (
}WW 
returnYY 
EncodingYY 
.YY  
UTF8YY  $
.YY$ %
	GetStringYY% .
(YY. /
msoYY/ 2
.YY2 3
ToArrayYY3 :
(YY: ;
)YY; <
)YY< =
;YY= >
}ZZ 
}[[ 	
}]] 
}^^ �;
OC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\ConfiguracaoServico.cs
	namespace,, 	
NFe,,
 
.,, 
Utils,, 
{-- 
public.. 

sealed.. 
class.. 
ConfiguracaoServico.. +
{// 
private00 
static00 
volatile00 
ConfiguracaoServico00  3

_instancia004 >
;00> ?
private11 
static11 
readonly11 
object11  &
SyncRoot11' /
=110 1
new112 5
object116 <
(11< =
)11= >
;11> ?
private22 
string22 
_diretorioSchemas22 (
;22( )
private33 
bool33 
_salvarXmlServicos33 '
;33' (
private55 
ConfiguracaoServico55 #
(55# $
)55$ %
{66 	
Certificado77 
=77 
new77 #
ConfiguracaoCertificado77 5
(775 6
)776 7
;777 8
}88 	
public== #
ConfiguracaoCertificado== &
Certificado==' 2
{==3 4
get==5 8
;==8 9
set==: =
;=== >
}==? @
publicBB 
intBB 
TimeOutBB 
{BB 
getBB  
;BB  !
setBB" %
;BB% &
}BB' (
publicGG 
	EstadoNfeGG 
cUFGG 
{GG 
getGG "
;GG" #
setGG$ '
;GG' (
}GG) *
publicLL 
TipoAmbienteLL 
tpAmbLL !
{LL" #
getLL$ '
;LL' (
setLL) ,
;LL, -
}LL. /
publicQQ 
TipoEmissaoQQ 
tpEmisQQ !
{QQ" #
getQQ$ '
;QQ' (
setQQ) ,
;QQ, -
}QQ. /
publicVV 
ModeloDocumentoVV 
ModeloDocumentoVV .
{VV/ 0
getVV1 4
;VV4 5
setVV6 9
;VV9 :
}VV; <
public[[ 
VersaoServico[[ /
#VersaoRecepcaoEventoCceCancelamento[[ @
{[[A B
get[[C F
;[[F G
set[[H K
;[[K L
}[[M N
public`` 
VersaoServico`` $
VersaoRecepcaoEventoEpec`` 5
{``6 7
get``8 ;
;``; <
set``= @
;``@ A
}``B C
publicee 
VersaoServicoee 8
,VersaoRecepcaoEventoManifestacaoDestinatarioee I
{eeJ K
geteeL O
;eeO P
seteeQ T
;eeT U
}eeV W
publicjj 
VersaoServicojj 
VersaoNfeRecepcaojj .
{jj/ 0
getjj1 4
;jj4 5
setjj6 9
;jj9 :
}jj; <
publicoo 
VersaoServicooo  
VersaoNfeRetRecepcaooo 1
{oo2 3
getoo4 7
;oo7 8
setoo9 <
;oo< =
}oo> ?
publictt 
VersaoServicott %
VersaoNfeConsultaCadastrott 6
{tt7 8
gettt9 <
;tt< =
settt> A
;ttA B
}ttC D
publicyy 
VersaoServicoyy !
VersaoNfeInutilizacaoyy 2
{yy3 4
getyy5 8
;yy8 9
setyy: =
;yy= >
}yy? @
public~~ 
VersaoServico~~ &
VersaoNfeConsultaProtocolo~~ 7
{~~8 9
get~~: =
;~~= >
set~~? B
;~~B C
}~~D E
public
�� 
VersaoServico
�� $
VersaoNfeStatusServico
�� 3
{
��4 5
get
��6 9
;
��9 :
set
��; >
;
��> ?
}
��@ A
public
�� 
VersaoServico
�� "
VersaoNFeAutorizacao
�� 1
{
��2 3
get
��4 7
;
��7 8
set
��9 <
;
��< =
}
��> ?
public
�� 
VersaoServico
�� %
VersaoNFeRetAutorizacao
�� 4
{
��5 6
get
��7 :
;
��: ;
set
��< ?
;
��? @
}
��A B
public
�� 
VersaoServico
�� &
VersaoNFeDistribuicaoDFe
�� 5
{
��6 7
get
��8 ;
;
��; <
set
��= @
;
��@ A
}
��B C
public
�� 
VersaoServico
�� #
VersaoNfeConsultaDest
�� 2
{
��3 4
get
��5 8
;
��8 9
set
��: =
;
��= >
}
��? @
public
�� 
VersaoServico
�� !
VersaoNfeDownloadNF
�� 0
{
��1 2
get
��3 6
;
��6 7
set
��8 ;
;
��; <
}
��= >
public
�� 
VersaoServico
�� '
VersaoNfceAministracaoCSC
�� 6
{
��7 8
get
��9 <
;
��< =
set
��> A
;
��A B
}
��C D
public
�� "
SecurityProtocolType
�� #"
ProtocoloDeSeguranca
��$ 8
{
��9 :
get
��; >
;
��> ?
set
��@ C
;
��C D
}
��E F
public
�� 
string
�� 
DiretorioSchemas
�� &
{
�� 	
get
�� 
{
�� 
return
�� 
_diretorioSchemas
�� *
;
��* +
}
��, -
set
�� 
{
�� 
if
�� 
(
�� 
!
�� 
string
�� 
.
�� 
IsNullOrEmpty
�� )
(
��) *
value
��* /
)
��/ 0
&&
��1 3
!
��4 5
	Directory
��5 >
.
��> ?
Exists
��? E
(
��E F
value
��F K
)
��K L
)
��L M
throw
�� 
new
�� 
	Exception
�� '
(
��' (
$str
��( 4
+
��5 6
value
��7 <
+
��= >
$str
��? Q
)
��Q R
;
��R S
_diretorioSchemas
�� !
=
��" #
value
��$ )
;
��) *
}
�� 
}
�� 	
public
�� 
bool
�� 
SalvarXmlServicos
�� %
{
�� 	
get
�� 
{
�� 
return
��  
_salvarXmlServicos
�� +
;
��+ ,
}
��- .
set
�� 
{
�� 
if
�� 
(
�� 
!
�� 
value
�� 
)
��  
DiretorioSalvarXml
�� &
=
��' (
$str
��) +
;
��+ , 
_salvarXmlServicos
�� "
=
��# $
value
��% *
;
��* +
}
�� 
}
�� 	
public
�� 
string
��  
DiretorioSalvarXml
�� (
{
��) *
get
��+ .
;
��. /
set
��0 3
;
��3 4
}
��5 6
public
�� 
static
�� !
ConfiguracaoServico
�� )
	Instancia
��* 3
{
�� 	
get
�� 
{
�� 
if
�� 
(
�� 

_instancia
�� 
!=
�� !
null
��" &
)
��& '
return
��( .

_instancia
��/ 9
;
��9 :
lock
�� 
(
�� 
SyncRoot
�� 
)
�� 
{
�� 
if
�� 
(
�� 

_instancia
�� "
!=
��# %
null
��& *
)
��* +
return
��, 2

_instancia
��3 =
;
��= >

_instancia
�� 
=
��  
new
��! $!
ConfiguracaoServico
��% 8
(
��8 9
)
��9 :
;
��: ;
}
�� 
return
�� 

_instancia
�� !
;
��! "
}
�� 
}
�� 	
}
�� 
}�� �
WC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\ConsultaCadastro\ExtConsCad.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
ConsultaCadastro%% $
{&& 
public'' 

static'' 
class'' 

ExtConsCad'' "
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
ConsCad..1 8
pedConsulta..9 D
)..D E
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
pedConsulta002 =
)00= >
;00> ?
}11 	
}22 
}33 �	
ZC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\ConsultaCadastro\ExtretConsCad.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
ConsultaCadastro%% $
{&& 
public'' 

static'' 
class'' 
ExtretConsCad'' %
{(( 
public// 
static// 

retConsCad//  
CarregarDeXmlString//! 4
(//4 5
this//5 9

retConsCad//: D

retConsCad//E O
,//O P
string//Q W
	xmlString//X a
)//a b
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2

retConsCad112 <
>11< =
(11= >
	xmlString11> G
)11G H
;11H I
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0

retConsCad991 ;

retConsCad99< F
)99F G
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2

retConsCad;;2 <
);;< =
;;;= >
}<< 	
}== 
}>> �
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Consulta\ExtconsSitNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Consulta%% 
{&& 
public'' 

static'' 
class'' 
ExtconsSitNFe'' %
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0

consSitNFe..1 ;
pedConsulta..< G
)..G H
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
pedConsulta002 =
)00= >
;00> ?
}11 	
}22 
}33 �	
UC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Consulta\ExtretConsSitNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Consulta%% 
{&& 
public'' 

static'' 
class'' 
ExtretConsSitNFe'' (
{(( 
public// 
static// 
retConsSitNFe// #
CarregarDeXmlString//$ 7
(//7 8
this//8 <
retConsSitNFe//= J
retConsSitNFe//K X
,//X Y
string//Z `
	xmlString//a j
)//j k
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retConsSitNFe112 ?
>11? @
(11@ A
	xmlString11A J
)11J K
;11K L
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retConsSitNFe991 >
retConsSitNFe99? L
)99L M
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retConsSitNFe;;2 ?
);;? @
;;;@ A
}<< 	
}== 
}>> �
ZC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\DistribuicaoDFe\ExtdistDFeInt2.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
DistribuicaoDFe%% #
{&& 
public'' 

static'' 
class'' 
ExtdistDFeInt2'' &
{(( 
public// 
static// 
string// 
ObterXmlString// +
(//+ ,
this//, 0
distDFeInt2//1 <
pedDistDFeInt//= J
)//J K
{00 	
return11 

FuncoesXml11 
.11 
ClasseParaXmlString11 1
(111 2
pedDistDFeInt112 ?
)11? @
;11@ A
}22 	
}33 
}44 �
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\DistribuicaoDFe\ExtdistDFeInt.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
DistribuicaoDFe%% #
{&& 
public'' 

static'' 
class'' 
ExtdistDFeInt'' %
{(( 
public// 
static// 
string// 
ObterXmlString// +
(//+ ,
this//, 0

distDFeInt//1 ;
pedDistDFeInt//< I
)//I J
{00 	
return11 

FuncoesXml11 
.11 
ClasseParaXmlString11 1
(111 2
pedDistDFeInt112 ?
)11? @
;11@ A
}22 	
}33 
}44 �	
]C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\DistribuicaoDFe\ExtretDistDFeInt2.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
DistribuicaoDFe%% #
{&& 
public'' 

static'' 
class'' 
ExtretDistDFeInt2'' )
{(( 
public// 
static// 
retDistDFeInt2// $
CarregarDeXmlString//% 8
(//8 9
this//9 =
retDistDFeInt2//> L
retDistDFeInt//M Z
,//Z [
string//\ b
	xmlString//c l
)//l m
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retDistDFeInt2112 @
>11@ A
(11A B
	xmlString11B K
)11K L
;11L M
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retDistDFeInt2991 ?
retDistDFeInt99@ M
)99M N
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retDistDFeInt;;2 ?
);;? @
;;;@ A
}<< 	
}== 
}>> �	
\C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\DistribuicaoDFe\ExtretDistDFeInt.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
DistribuicaoDFe%% #
{&& 
public'' 

static'' 
class'' 
ExtretDistDFeInt'' (
{(( 
public// 
static// 
retDistDFeInt// #
CarregarDeXmlString//$ 7
(//7 8
this//8 <
retDistDFeInt//= J
retDistDFeInt//K X
,//X Y
string//Z `
	xmlString//a j
)//j k
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retDistDFeInt112 ?
>11? @
(11@ A
	xmlString11A J
)11J K
;11K L
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retDistDFeInt991 >
retDistDFeInt99? L
)99L M
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retDistDFeInt;;2 ?
);;? @
;;;@ A
}<< 	
}== 
}>> �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Download\ExtdownloadNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Download%% 
{&& 
public'' 

static'' 
class'' 
ExtdownloadNFe'' &
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
downloadNFe..1 <
pedDownload..= H
)..H I
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
pedDownload002 =
)00= >
;00> ?
}11 	
}22 
}33 �	
VC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Download\ExtretDownloadNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Download%% 
{&& 
public'' 

static'' 
class'' 
ExtretDownloadNFe'' )
{(( 
public// 
static// 
retDownloadNFe// $
CarregarDeXmlString//% 8
(//8 9
this//9 =
retDownloadNFe//> L
retDownloadNFe//M [
,//[ \
string//] c
	xmlString//d m
)//m n
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retDownloadNFe112 @
>11@ A
(11A B
	xmlString11B K
)11K L
;11L M
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retDownloadNFe991 ?
retDownloadNFe99@ N
)99N O
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retDownloadNFe;;2 @
);;@ A
;;;A B
}<< 	
}== 
}>> �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Email\ConfiguracaoEmail.cs
	namespace"" 	
NFe""
 
."" 
Utils"" 
."" 
Email"" 
{## 
public'' 

class'' 
ConfiguracaoEmail'' "
{(( 
public)) 
ConfiguracaoEmail))  
())  !
string))! '
email))( -
,))- .
string))/ 5
senha))6 ;
,)); <
string))= C
assunto))D K
,))K L
string))M S
mensagem))T \
,))\ ]
string))^ d
servidorSmtp))e q
,))q r
int))s v
porta))w |
,))| }
bool	))~ �
ssl
))� �
=
))� �
true
))� �
,
))� �
bool
))� �
mensagemHtml
))� �
=
))� �
false
))� �
,
))� �
int
))� �
timeout
))� �
=
))� �
$num
))� �
,
))� �
bool
))� �

assincrono
))� �
=
))� �
true
))� �
)
))� �
{** 	
Email++ 
=++ 
email++ 
;++ 
Senha,, 
=,, 
senha,, 
;,, 
Assunto-- 
=-- 
assunto-- 
;-- 
Mensagem.. 
=.. 
mensagem.. 
;..  
ServidorSmtp// 
=// 
servidorSmtp// '
;//' (
Porta00 
=00 
porta00 
;00 
Ssl11 
=11 
ssl11 
;11 
MensagemEmHtml22 
=22 
mensagemHtml22 )
;22) *
Timeout33 
=33 
timeout33 
;33 

Assincrono44 
=44 

assincrono44 #
;44# $
}55 	
private:: 
ConfiguracaoEmail:: !
(::! "
)::" #
{;; 	
}<< 	
publicAA 
stringAA 
EmailAA 
{AA 
getAA !
;AA! "
setAA# &
;AA& '
}AA( )
publicFF 
stringFF 
SenhaFF 
{FF 
getFF !
;FF! "
setFF# &
;FF& '
}FF( )
publicKK 
stringKK 
AssuntoKK 
{KK 
getKK  #
;KK# $
setKK% (
;KK( )
}KK* +
publicPP 
stringPP 
MensagemPP 
{PP  
getPP! $
;PP$ %
setPP& )
;PP) *
}PP+ ,
publicUU 
stringUU 
ServidorSmtpUU "
{UU# $
getUU% (
;UU( )
setUU* -
;UU- .
}UU/ 0
publicZZ 
intZZ 
PortaZZ 
{ZZ 
getZZ 
;ZZ 
setZZ  #
;ZZ# $
}ZZ% &
public__ 
bool__ 
Ssl__ 
{__ 
get__ 
;__ 
set__ "
;__" #
}__$ %
publicdd 
booldd 
MensagemEmHtmldd "
{dd# $
getdd% (
;dd( )
setdd* -
;dd- .
}dd/ 0
publicii 
intii 
Timeoutii 
{ii 
getii  
;ii  !
setii" %
;ii% &
}ii' (
publicoo 
booloo 

Assincronooo 
{oo  
getoo! $
;oo$ %
setoo& )
;oo) *
}oo+ ,
}pp 
}qq �T
NC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Email\EmailBuilder.cs
	namespace 	
NFe
 
. 
Utils 
. 
Email 
{		 
public

 

delegate

 
void

 
ErroAoEnviarEmail

 *
(

* +
	Exception

+ 4
erro

5 9
)

9 :
;

: ;
public 

sealed 
class 
EmailBuilder $
{ 
public 
event 
EventHandler !
AntesDeEnviarEmail" 4
;4 5
public 
event 
EventHandler !
DepoisDeEnviarEmail" 5
;5 6
public 
event 
ErroAoEnviarEmail &
ErroAoEnviarEmail' 8
=9 :
delegate; C
{D E
}F G
;G H
private 
readonly 
ConfiguracaoEmail *
_configuracaoEmail+ =
;= >
private 
readonly 
List 
< 
string $
>$ %
_destinatarios& 4
;4 5
private 
readonly 
List 
< 
string $
>$ %
_anexos& -
;- .
public 
EmailBuilder 
( 
ConfiguracaoEmail -
configuracaoEmail. ?
)? @
{ 	
_configuracaoEmail 
=  
configuracaoEmail! 2
;2 3
_destinatarios 
= 
new  
List! %
<% &
string& ,
>, -
(- .
). /
;/ 0
_anexos 
= 
new 
List 
< 
string %
>% &
(& '
)' (
;( )
} 	
public!! 
EmailBuilder!! !
AdicionarDestinatario!! 1
(!!1 2
string!!2 8
email!!9 >
)!!> ?
{"" 	
if## 
(## 
!## 
EmailValido## 
(## 
email## "
)##" #
)### $
throw$$ 
new$$ 
ArgumentException$$ +
($$+ ,
$str$$, P
)$$P Q
;$$Q R
_destinatarios&& 
.&& 
Add&& 
(&& 
email&& $
)&&$ %
;&&% &
return'' 
this'' 
;'' 
}(( 	
public// 
EmailBuilder// 
AdicionarAnexo// *
(//* +
string//+ 1
pathArquivo//2 =
)//= >
{00 	
_anexos11 
.11 
Add11 
(11 
pathArquivo11 #
)11# $
;11$ %
return22 
this22 
;22 
}33 	
public88 
void88 
Enviar88 
(88 
)88 
{99 	
Verificacao:: 
(:: 
):: 
;:: 
var<< 
mensagem<< 
=<< 
new<< 
MailMessage<< *
{== 
From>> 
=>> 
new>> 
MailAddress>> &
(>>& '
_configuracaoEmail>>' 9
.>>9 :
Email>>: ?
)>>? @
,>>@ A
Subject?? 
=?? 
_configuracaoEmail?? ,
.??, -
Assunto??- 4
????5 7
string??8 >
.??> ?
Empty??? D
,??D E
Body@@ 
=@@ 
_configuracaoEmail@@ )
.@@) *
Mensagem@@* 2
??@@3 5
string@@6 <
.@@< =
Empty@@= B
,@@B C

IsBodyHtmlAA 
=AA 
_configuracaoEmailAA /
.AA/ 0
MensagemEmHtmlAA0 >
}CC 
;CC 
_destinatariosDD 
.DD 
ForEachDD "
(DD" #
mensagemDD# +
.DD+ ,
ToDD, .
.DD. /
AddDD/ 2
)DD2 3
;DD3 4
_anexosFF 
.FF 
ForEachFF 
(FF 
aFF 
=>FF  
{FF! "
mensagemFF# +
.FF+ ,
AttachmentsFF, 7
.FF7 8
AddFF8 ;
(FF; <
newFF< ?

AttachmentFF@ J
(FFJ K
aFFK L
,FFL M
MediaTypeNamesFFN \
.FF\ ]
ApplicationFF] h
.FFh i
OctetFFi n
)FFn o
)FFo p
;FFp q
}FFr s
)FFs t
;FFt u
varHH 
clienteHH 
=HH 
newHH 

SmtpClientHH (
(HH( )
_configuracaoEmailHH) ;
.HH; <
ServidorSmtpHH< H
,HHH I
_configuracaoEmailHHJ \
.HH\ ]
PortaHH] b
)HHb c
{II 
	EnableSslJJ 
=JJ 
_configuracaoEmailJJ .
.JJ. /
SslJJ/ 2
,JJ2 3!
UseDefaultCredentialsKK %
=KK& '
falseKK( -
,KK- .
DeliveryMethodLL 
=LL  
SmtpDeliveryMethodLL! 3
.LL3 4
NetworkLL4 ;
,LL; <
}MM 
;MM 
clienteNN 
.NN 
SendCompletedNN !
+=NN" $
(NN% &
senderNN& ,
,NN, -
argsNN. 2
)NN2 3
=>NN4 6
{OO 
ifPP 
(PP 
argsPP 
.PP 
ErrorPP 
!=PP !
nullPP" &
)PP& '
ErroAoEnviarEmailQQ %
(QQ% &
argsQQ& *
.QQ* +
ErrorQQ+ 0
)QQ0 1
;QQ1 2
elseRR !
OnDepoisDeEnviarEmailSS )
(SS) *
)SS* +
;SS+ ,
mensagemTT 
.TT 
DisposeTT  
(TT  !
)TT! "
;TT" #
clienteUU 
.UU 
DisposeUU 
(UU  
)UU  !
;UU! "
}VV 
;VV 
varXX 
credXX 
=XX 
newXX 
NetworkCredentialXX ,
(XX, -
_configuracaoEmailXX- ?
.XX? @
EmailXX@ E
,XXE F
_configuracaoEmailYY "
.YY" #
SenhaYY# (
)YY( )
;YY) *
clienteZZ 
.ZZ 
CredentialsZZ 
=ZZ  !
credZZ" &
;ZZ& '
cliente\\ 
.\\ 
Timeout\\ 
=\\ 
_configuracaoEmail\\ 0
.\\0 1
Timeout\\1 8
==\\9 ;
$num\\< =
?\\> ?
cliente\\@ G
.\\G H
Timeout\\H O
:\\P Q
_configuracaoEmail\\R d
.\\d e
Timeout\\e l
;\\l m 
OnAntesDeEnviarEmail]]  
(]]  !
)]]! "
;]]" #
if^^ 
(^^ 
_configuracaoEmail^^ "
.^^" #

Assincrono^^# -
)^^- .
cliente__ 
.__ 
	SendAsync__ !
(__! "
mensagem__" *
,__* +
null__, 0
)__0 1
;__1 2
else`` 
{aa 
clientebb 
.bb 
Sendbb 
(bb 
mensagembb %
)bb% &
;bb& '!
OnDepoisDeEnviarEmailcc %
(cc% &
)cc& '
;cc' (
mensagemdd 
.dd 
Disposedd  
(dd  !
)dd! "
;dd" #
clienteee 
.ee 
Disposeee 
(ee  
)ee  !
;ee! "
}ff 
}hh 	
privatemm 
voidmm 
Verificacaomm  
(mm  !
)mm! "
{nn 	
ifoo 
(oo 
_configuracaoEmailoo "
==oo# %
nulloo& *
)oo* +
throwoo, 1
newoo2 5%
InvalidOperationExceptionoo6 O
(ooO P
$str	ooP �
)
oo� �
;
oo� �
ifqq 
(qq 
stringqq 
.qq 
IsNullOrEmptyqq $
(qq$ %
_configuracaoEmailqq% 7
.qq7 8
Emailqq8 =
)qq= >
)qq> ?
throwrr 
newrr 
ArgumentExceptionrr +
(rr+ ,
$strrr, V
)rrV W
;rrW X
iftt 
(tt 
!tt 
EmailValidott 
(tt 
_configuracaoEmailtt /
.tt/ 0
Emailtt0 5
)tt5 6
)tt6 7
throwuu 
newuu 
ArgumentExceptionuu +
(uu+ ,
$struu, M
)uuM N
;uuN O
ifww 
(ww 
stringww 
.ww 
IsNullOrEmptyww $
(ww$ %
_configuracaoEmailww% 7
.ww7 8
Senhaww8 =
)ww= >
)ww> ?
throwxx 
newxx 
ArgumentExceptionxx +
(xx+ ,
$strxx, _
)xx_ `
;xx` a
ifzz 
(zz 
stringzz 
.zz 
IsNullOrEmptyzz $
(zz$ %
_configuracaoEmailzz% 7
.zz7 8
ServidorSmtpzz8 D
)zzD E
)zzE F
throw{{ 
new{{ 
ArgumentException{{ +
({{+ ,
$str{{, N
){{N O
;{{O P
}|| 	
public
�� 
static
�� 
bool
�� 
EmailValido
�� &
(
��& '
string
��' -
email
��. 3
)
��3 4
{
�� 	
var
�� 
rg
�� 
=
�� 
new
�� 
Regex
�� 
(
�� 
$str
�� 
)�� �
;��� �
return
�� 
rg
�� 
.
�� 
IsMatch
�� 
(
�� 
email
�� #
)
��# $
;
��$ %
}
�� 	
private
�� 
void
�� "
OnAntesDeEnviarEmail
�� )
(
��) *
)
��* +
{
�� 	
if
�� 
(
��  
AntesDeEnviarEmail
�� "
!=
��# %
null
��& *
)
��* + 
AntesDeEnviarEmail
��, >
.
��> ?
Invoke
��? E
(
��E F
this
��F J
,
��J K
	EventArgs
��L U
.
��U V
Empty
��V [
)
��[ \
;
��\ ]
}
�� 	
private
�� 
void
�� #
OnDepoisDeEnviarEmail
�� *
(
��* +
)
��+ ,
{
�� 	
if
�� 
(
�� !
DepoisDeEnviarEmail
�� #
!=
��$ &
null
��' +
)
��+ ,!
DepoisDeEnviarEmail
��- @
.
��@ A
Invoke
��A G
(
��G H
this
��H L
,
��L M
	EventArgs
��N W
.
��W X
Empty
��X ]
)
��] ^
;
��^ _
}
�� 	
}
�� 
}�� �
OC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Evento\ExtenvEvento.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Evento%% 
{&& 
public'' 

static'' 
class'' 
ExtenvEvento'' $
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
	envEvento..1 :
	pedEvento..; D
)..D E
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
	pedEvento002 ;
)00; <
;00< =
}11 	
}22 
}33 �
LC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Evento\Extevento.cs
	namespace'' 	
NFe''
 
.'' 
Utils'' 
.'' 
Evento'' 
{(( 
public)) 

static)) 
class)) 
	Extevento)) !
{** 
public00 
static00 
string00 
ObterXmlString00 +
(00+ ,
this00, 0
evento001 7
	pedEvento008 A
)00A B
{11 	
return22 

FuncoesXml22 
.22 
ClasseParaXmlString22 1
(221 2
	pedEvento222 ;
)22; <
;22< =
}33 	
public;; 
static;; 
evento;; 
Assina;; #
(;;# $
this;;$ (
evento;;) /
evento;;0 6
,;;6 7
X509Certificate2;;8 H
certificadoDigital;;I [
);;[ \
{<< 	
var== 
eventoLocal== 
=== 
evento== $
;==$ %
if>> 
(>> 
eventoLocal>> 
.>> 
	infEvento>> %
.>>% &
Id>>& (
==>>) +
null>>, 0
)>>0 1
throw?? 
new?? 
	Exception?? #
(??# $
$str??$ d
)??d e
;??e f
varAA 

assinaturaAA 
=AA 
	AssinadorAA &
.AA& '
ObterAssinaturaAA' 6
(AA6 7
eventoLocalAA7 B
,AAB C
eventoLocalAAD O
.AAO P
	infEventoAAP Y
.AAY Z
IdAAZ \
,AA\ ]
certificadoDigitalAA^ p
)AAp q
;AAq r
eventoLocalBB 
.BB 
	SignatureBB !
=BB" #

assinaturaBB$ .
;BB. /
returnCC 
eventoLocalCC 
;CC 
}DD 	
}EE 
}FF �	
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Evento\ExtretEnvEvento.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Evento%% 
{&& 
public'' 

static'' 
class'' 
ExtretEnvEvento'' '
{(( 
public// 
static// 
retEnvEvento// "
CarregarDeXmlString//# 6
(//6 7
this//7 ;
retEnvEvento//< H
retEnvEvento//I U
,//U V
string//W ]
	xmlString//^ g
)//g h
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retEnvEvento112 >
>11> ?
(11? @
	xmlString11@ I
)11I J
;11J K
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retEnvEvento991 =
retEnvEvento99> J
)99J K
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retEnvEvento;;2 >
);;> ?
;;;? @
}<< 	
}== 
}>> �
`C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Excecoes\FabricaComunicacaoException.cs
	namespace'' 	
NFe''
 
.'' 
Utils'' 
.'' 
Excecoes'' 
{(( 
public,, 

static,, 
class,, '
FabricaComunicacaoException,, 3
{-- 
public77 
static77 
	Exception77 
ObterException77  .
(77. /

ServicoNFe77/ 9
servico77: A
,77A B
WebException77C O
webException77P \
)77\ ]
{88 	
if99 
(99 %
ListaComunicacaoException99 )
.99) *
Contains99* 2
(992 3
webException993 ?
.99? @
Status99@ F
)99F G
)99G H
return:: 
new::  
ComunicacaoException:: /
(::/ 0
servico::0 7
,::7 8
webException::9 E
.::E F
Message::F M
)::M N
;::N O
return;; 
webException;; 
;;;  
}<< 	
privateAA 
staticAA 
readonlyAA 
ListAA  $
<AA$ %
WebExceptionStatusAA% 7
>AA7 8%
ListaComunicacaoExceptionAA9 R
=AAS T
newAAU X
ListAAY ]
<AA] ^
WebExceptionStatusAA^ p
>AAp q
{BB 	
WebExceptionStatusCC 
.CC 
CacheEntryNotFoundCC 1
,CC1 2
WebExceptionStatusDD 
.DD 
ConnectFailureDD -
,DD- .
WebExceptionStatusEE 
.EE 
ConnectionClosedEE /
,EE/ 0
WebExceptionStatusFF 
.FF 
KeepAliveFailureFF /
,FF/ 0
WebExceptionStatusGG 
.GG &
MessageLengthLimitExceededGG 9
,GG9 :
WebExceptionStatusHH 
.HH !
NameResolutionFailureHH 4
,HH4 5
WebExceptionStatusII 
.II 
PendingII &
,II& '
WebExceptionStatusJJ 
.JJ 
PipelineFailureJJ .
,JJ. /
WebExceptionStatusLL 
.LL &
ProxyNameResolutionFailureLL 9
,LL9 :
WebExceptionStatusMM 
.MM 
ReceiveFailureMM -
,MM- .
WebExceptionStatusNN 
.NN 
RequestCanceledNN .
,NN. /
WebExceptionStatusOO 
.OO *
RequestProhibitedByCachePolicyOO =
,OO= >
WebExceptionStatusPP 
.PP $
RequestProhibitedByProxyPP 7
,PP7 8
WebExceptionStatusRR 
.RR 
SendFailureRR *
,RR* +
WebExceptionStatusSS 
.SS #
ServerProtocolViolationSS 6
,SS6 7
WebExceptionStatusUU 
.UU 
TimeoutUU &
,UU& '
WebExceptionStatusWW 
.WW 
UnknownErrorWW +
}XX 	
;XX	 

}YY 
}ZZ �
]C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Excecoes\ValidacaoSchemaException.cs
	namespace$$ 	
NFe$$
 
.$$ 
Utils$$ 
.$$ 
Excecoes$$ 
{%% 
public,, 

class,, $
ValidacaoSchemaException,, )
:,,* +
	Exception,,, 5
{-- 
public22 $
ValidacaoSchemaException22 '
(22' (
string22( .
message22/ 6
)226 7
:228 9
base22: >
(22> ?
string22? E
.22E F
Format22F L
(22L M
$str22M h
,22h i
message22j q
)22q r
)22r s
{22t u
}22v w
}33 
}44 �
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Excecoes\ComunicacaoException.cs
	namespace$$ 	
NFe$$
 
.$$ 
Utils$$ 
.$$ 
Excecoes$$ 
{%% 
public)) 

class))  
ComunicacaoException)) %
:))& '
	Exception))( 1
{** 
public00  
ComunicacaoException00 #
(00# $

ServicoNFe00$ .
servico00/ 6
,006 7
string008 >
message00? F
)00F G
:00H I
base00J N
(00N O
string00O U
.00U V
Format00V \
(00\ ]
$str	00] �
,
00� �
servico
00� �
,
00� �
message
00� �
)
00� �
)
00� �
{
00� �
}
00� �
}11 
}22 ��
bC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\InformacoesSuplementares\ExtinfNFeSupl.cs
	namespace,, 	
NFe,,
 
.,, 
Utils,, 
.,, $
InformacoesSuplementares,, ,
{-- 
internal.. 
class.. '
EnderecoConsultaPublicaNfce.. .
{// 
public00 '
EnderecoConsultaPublicaNfce00 *
(00* +
	EstadoNfe00+ 4
estado005 ;
,00; <
TipoAmbiente00= I
tipoAmbiente00J V
,00V W"
TipoUrlConsultaPublica00X n#
tipoUrlConsultaPublica	00o �
,
00� �
string
00� �
url
00� �
)
00� �
{11 	
TipoAmbiente22 
=22 
tipoAmbiente22 '
;22' (
Estado33 
=33 
estado33 
;33 "
TipoUrlConsultaPublica44 "
=44# $"
tipoUrlConsultaPublica44% ;
;44; <
Url55 
=55 
url55 
;55 
}66 	
public88 
TipoAmbiente88 
TipoAmbiente88 (
{88) *
get88+ .
;88. /
	protected880 9
set88: =
;88= >
}88? @
public99 
	EstadoNfe99 
Estado99 
{99  !
get99" %
;99% &
	protected99' 0
set991 4
;994 5
}996 7
public:: "
TipoUrlConsultaPublica:: %"
TipoUrlConsultaPublica::& <
{::= >
get::? B
;::B C
	protected::D M
set::N Q
;::Q R
}::S T
public;; 
string;; 
Url;; 
{;; 
get;; 
;;;  
	protected;;! *
set;;+ .
;;;. /
};;0 1
}<< 
publicAA 

staticAA 
classAA 
ExtinfNFeSuplAA %
{BB 
privateCC 
staticCC 
readonlyCC 
ListCC  $
<CC$ %'
EnderecoConsultaPublicaNfceCC% @
>CC@ A
EndQrCodeNfceCCB O
;CCO P
staticEE 
ExtinfNFeSuplEE 
(EE 
)EE 
{FF 	
EndQrCodeNfceGG 
=GG 
CarregarUrlsGG (
(GG( )
)GG) *
;GG* +
}HH 	
privateJJ 
staticJJ 
ListJJ 
<JJ '
EnderecoConsultaPublicaNfceJJ 7
>JJ7 8
CarregarUrlsJJ9 E
(JJE F
)JJF G
{KK 	
varLL 
endQrCodeNfceLL 
=LL 
newLL  #
ListLL$ (
<LL( )'
EnderecoConsultaPublicaNfceLL) D
>LLD E
{MM 
newNN '
EnderecoConsultaPublicaNfceNN /
(NN/ 0
	EstadoNfeNN0 9
.NN9 :
ACNN: <
,NN< =
TipoAmbienteNN> J
.NNJ K

taProducaoNNK U
,NNU V"
TipoUrlConsultaPublicaNNW m
.NNm n
	UrlQrCodeNNn w
,NNw x
$str	NNy �
)
NN� �
,
NN� �
newOO '
EnderecoConsultaPublicaNfceOO /
(OO/ 0
	EstadoNfeOO0 9
.OO9 :
ACOO: <
,OO< =
TipoAmbienteOO> J
.OOJ K

taProducaoOOK U
,OOU V"
TipoUrlConsultaPublicaOOW m
.OOm n
UrlConsultaOOn y
,OOy z
$str	OO{ �
)
OO� �
,
OO� �
newPP '
EnderecoConsultaPublicaNfcePP /
(PP/ 0
	EstadoNfePP0 9
.PP9 :
ACPP: <
,PP< =
TipoAmbientePP> J
.PPJ K
taHomologacaoPPK X
,PPX Y"
TipoUrlConsultaPublicaPPZ p
.PPp q
	UrlQrCodePPq z
,PPz {
$str	PP| �
)
PP� �
,
PP� �
newQQ '
EnderecoConsultaPublicaNfceQQ /
(QQ/ 0
	EstadoNfeQQ0 9
.QQ9 :
ACQQ: <
,QQ< =
TipoAmbienteQQ> J
.QQJ K
taHomologacaoQQK X
,QQX Y"
TipoUrlConsultaPublicaQQZ p
.QQp q
UrlConsultaQQq |
,QQ| }
$str	QQ~ �
)
QQ� �
,
QQ� �
newSS '
EnderecoConsultaPublicaNfceSS /
(SS/ 0
	EstadoNfeSS0 9
.SS9 :
ALSS: <
,SS< =
TipoAmbienteSS> J
.SSJ K

taProducaoSSK U
,SSU V"
TipoUrlConsultaPublicaSSW m
.SSm n
	UrlQrCodeSSn w
,SSw x
$str	SSy �
)
SS� �
,
SS� �
newTT '
EnderecoConsultaPublicaNfceTT /
(TT/ 0
	EstadoNfeTT0 9
.TT9 :
ALTT: <
,TT< =
TipoAmbienteTT> J
.TTJ K

taProducaoTTK U
,TTU V"
TipoUrlConsultaPublicaTTW m
.TTm n
UrlConsultaTTn y
,TTy z
$str	TT{ �
)
TT� �
,
TT� �
newUU '
EnderecoConsultaPublicaNfceUU /
(UU/ 0
	EstadoNfeUU0 9
.UU9 :
ALUU: <
,UU< =
TipoAmbienteUU> J
.UUJ K
taHomologacaoUUK X
,UUX Y"
TipoUrlConsultaPublicaUUZ p
.UUp q
	UrlQrCodeUUq z
,UUz {
$str	UU| �
)
UU� �
,
UU� �
newVV '
EnderecoConsultaPublicaNfceVV /
(VV/ 0
	EstadoNfeVV0 9
.VV9 :
ALVV: <
,VV< =
TipoAmbienteVV> J
.VVJ K
taHomologacaoVVK X
,VVX Y"
TipoUrlConsultaPublicaVVZ p
.VVp q
UrlConsultaVVq |
,VV| }
$str	VV~ �
)
VV� �
,
VV� �
newXX '
EnderecoConsultaPublicaNfceXX /
(XX/ 0
	EstadoNfeXX0 9
.XX9 :
AMXX: <
,XX< =
TipoAmbienteXX> J
.XXJ K

taProducaoXXK U
,XXU V"
TipoUrlConsultaPublicaXXW m
.XXm n
	UrlQrCodeXXn w
,XXw x
$str	XXy �
)
XX� �
,
XX� �
newYY '
EnderecoConsultaPublicaNfceYY /
(YY/ 0
	EstadoNfeYY0 9
.YY9 :
AMYY: <
,YY< =
TipoAmbienteYY> J
.YYJ K

taProducaoYYK U
,YYU V"
TipoUrlConsultaPublicaYYW m
.YYm n
UrlConsultaYYn y
,YYy z
$str	YY{ �
)
YY� �
,
YY� �
newZZ '
EnderecoConsultaPublicaNfceZZ /
(ZZ/ 0
	EstadoNfeZZ0 9
.ZZ9 :
AMZZ: <
,ZZ< =
TipoAmbienteZZ> J
.ZZJ K
taHomologacaoZZK X
,ZZX Y"
TipoUrlConsultaPublicaZZZ p
.ZZp q
	UrlQrCodeZZq z
,ZZz {
$str	ZZ| �
)
ZZ� �
,
ZZ� �
new[[ '
EnderecoConsultaPublicaNfce[[ /
([[/ 0
	EstadoNfe[[0 9
.[[9 :
AM[[: <
,[[< =
TipoAmbiente[[> J
.[[J K
taHomologacao[[K X
,[[X Y"
TipoUrlConsultaPublica[[Z p
.[[p q
UrlConsulta[[q |
,[[| }
$str	[[~ �
)
[[� �
,
[[� �
new]] '
EnderecoConsultaPublicaNfce]] /
(]]/ 0
	EstadoNfe]]0 9
.]]9 :
AP]]: <
,]]< =
TipoAmbiente]]> J
.]]J K

taProducao]]K U
,]]U V"
TipoUrlConsultaPublica]]W m
.]]m n
	UrlQrCode]]n w
,]]w x
$str	]]y �
)
]]� �
,
]]� �
new^^ '
EnderecoConsultaPublicaNfce^^ /
(^^/ 0
	EstadoNfe^^0 9
.^^9 :
AP^^: <
,^^< =
TipoAmbiente^^> J
.^^J K

taProducao^^K U
,^^U V"
TipoUrlConsultaPublica^^W m
.^^m n
UrlConsulta^^n y
,^^y z
$str	^^{ �
)
^^� �
,
^^� �
new__ '
EnderecoConsultaPublicaNfce__ /
(__/ 0
	EstadoNfe__0 9
.__9 :
AP__: <
,__< =
TipoAmbiente__> J
.__J K
taHomologacao__K X
,__X Y"
TipoUrlConsultaPublica__Z p
.__p q
	UrlQrCode__q z
,__z {
$str	__| �
)
__� �
,
__� �
new`` '
EnderecoConsultaPublicaNfce`` /
(``/ 0
	EstadoNfe``0 9
.``9 :
AP``: <
,``< =
TipoAmbiente``> J
.``J K
taHomologacao``K X
,``X Y"
TipoUrlConsultaPublica``Z p
.``p q
UrlConsulta``q |
,``| }
$str	``~ �
)
``� �
,
``� �
newbb '
EnderecoConsultaPublicaNfcebb /
(bb/ 0
	EstadoNfebb0 9
.bb9 :
BAbb: <
,bb< =
TipoAmbientebb> J
.bbJ K

taProducaobbK U
,bbU V"
TipoUrlConsultaPublicabbW m
.bbm n
	UrlQrCodebbn w
,bbw x
$str	bby �
)
bb� �
,
bb� �
newcc '
EnderecoConsultaPublicaNfcecc /
(cc/ 0
	EstadoNfecc0 9
.cc9 :
BAcc: <
,cc< =
TipoAmbientecc> J
.ccJ K

taProducaoccK U
,ccU V"
TipoUrlConsultaPublicaccW m
.ccm n
UrlConsultaccn y
,ccy z
$str	cc{ �
)
cc� �
,
cc� �
newdd '
EnderecoConsultaPublicaNfcedd /
(dd/ 0
	EstadoNfedd0 9
.dd9 :
BAdd: <
,dd< =
TipoAmbientedd> J
.ddJ K
taHomologacaoddK X
,ddX Y"
TipoUrlConsultaPublicaddZ p
.ddp q
	UrlQrCodeddq z
,ddz {
$str	dd| �
)
dd� �
,
dd� �
newee '
EnderecoConsultaPublicaNfceee /
(ee/ 0
	EstadoNfeee0 9
.ee9 :
BAee: <
,ee< =
TipoAmbienteee> J
.eeJ K
taHomologacaoeeK X
,eeX Y"
TipoUrlConsultaPublicaeeZ p
.eep q
UrlConsultaeeq |
,ee| }
$str	ee~ �
)
ee� �
,
ee� �
newgg '
EnderecoConsultaPublicaNfcegg /
(gg/ 0
	EstadoNfegg0 9
.gg9 :
DFgg: <
,gg< =
TipoAmbientegg> J
.ggJ K

taProducaoggK U
,ggU V"
TipoUrlConsultaPublicaggW m
.ggm n
	UrlQrCodeggn w
,ggw x
$str	ggy �
)
gg� �
,
gg� �
newhh '
EnderecoConsultaPublicaNfcehh /
(hh/ 0
	EstadoNfehh0 9
.hh9 :
DFhh: <
,hh< =
TipoAmbientehh> J
.hhJ K

taProducaohhK U
,hhU V"
TipoUrlConsultaPublicahhW m
.hhm n
UrlConsultahhn y
,hhy z
$str	hh{ �
)
hh� �
,
hh� �
newii '
EnderecoConsultaPublicaNfceii /
(ii/ 0
	EstadoNfeii0 9
.ii9 :
DFii: <
,ii< =
TipoAmbienteii> J
.iiJ K
taHomologacaoiiK X
,iiX Y"
TipoUrlConsultaPublicaiiZ p
.iip q
	UrlQrCodeiiq z
,iiz {
$str	ii| �
)
ii� �
,
ii� �
newjj '
EnderecoConsultaPublicaNfcejj /
(jj/ 0
	EstadoNfejj0 9
.jj9 :
DFjj: <
,jj< =
TipoAmbientejj> J
.jjJ K
taHomologacaojjK X
,jjX Y"
TipoUrlConsultaPublicajjZ p
.jjp q
UrlConsultajjq |
,jj| }
$str	jj~ �
)
jj� �
,
jj� �
newll '
EnderecoConsultaPublicaNfcell /
(ll/ 0
	EstadoNfell0 9
.ll9 :
GOll: <
,ll< =
TipoAmbientell> J
.llJ K

taProducaollK U
,llU V"
TipoUrlConsultaPublicallW m
.llm n
	UrlQrCodelln w
,llw x
$str	lly �
)
ll� �
,
ll� �
newmm '
EnderecoConsultaPublicaNfcemm /
(mm/ 0
	EstadoNfemm0 9
.mm9 :
GOmm: <
,mm< =
TipoAmbientemm> J
.mmJ K

taProducaommK U
,mmU V"
TipoUrlConsultaPublicammW m
.mmm n
UrlConsultammn y
,mmy z
$str	mm{ �
)
mm� �
,
mm� �
newnn '
EnderecoConsultaPublicaNfcenn /
(nn/ 0
	EstadoNfenn0 9
.nn9 :
GOnn: <
,nn< =
TipoAmbientenn> J
.nnJ K
taHomologacaonnK X
,nnX Y"
TipoUrlConsultaPublicannZ p
.nnp q
	UrlQrCodennq z
,nnz {
$str	nn| �
)
nn� �
,
nn� �
newoo '
EnderecoConsultaPublicaNfceoo /
(oo/ 0
	EstadoNfeoo0 9
.oo9 :
GOoo: <
,oo< =
TipoAmbienteoo> J
.ooJ K
taHomologacaoooK X
,ooX Y"
TipoUrlConsultaPublicaooZ p
.oop q
UrlConsultaooq |
,oo| }
$str	oo~ �
)
oo� �
,
oo� �
newqq '
EnderecoConsultaPublicaNfceqq /
(qq/ 0
	EstadoNfeqq0 9
.qq9 :
ESqq: <
,qq< =
TipoAmbienteqq> J
.qqJ K

taProducaoqqK U
,qqU V"
TipoUrlConsultaPublicaqqW m
.qqm n
	UrlQrCodeqqn w
,qqw x
$str	qqy �
)
qq� �
,
qq� �
newrr '
EnderecoConsultaPublicaNfcerr /
(rr/ 0
	EstadoNferr0 9
.rr9 :
ESrr: <
,rr< =
TipoAmbienterr> J
.rrJ K

taProducaorrK U
,rrU V"
TipoUrlConsultaPublicarrW m
.rrm n
UrlConsultarrn y
,rry z
$str	rr{ �
)
rr� �
,
rr� �
newss '
EnderecoConsultaPublicaNfcess /
(ss/ 0
	EstadoNfess0 9
.ss9 :
ESss: <
,ss< =
TipoAmbientess> J
.ssJ K
taHomologacaossK X
,ssX Y"
TipoUrlConsultaPublicassZ p
.ssp q
	UrlQrCodessq z
,ssz {
$str	ss| �
)
ss� �
,
ss� �
newtt '
EnderecoConsultaPublicaNfcett /
(tt/ 0
	EstadoNfett0 9
.tt9 :
EStt: <
,tt< =
TipoAmbientett> J
.ttJ K
taHomologacaottK X
,ttX Y"
TipoUrlConsultaPublicattZ p
.ttp q
UrlConsultattq |
,tt| }
$str	tt~ �
)
tt� �
,
tt� �
newvv '
EnderecoConsultaPublicaNfcevv /
(vv/ 0
	EstadoNfevv0 9
.vv9 :
MAvv: <
,vv< =
TipoAmbientevv> J
.vvJ K

taProducaovvK U
,vvU V"
TipoUrlConsultaPublicavvW m
.vvm n
	UrlQrCodevvn w
,vvw x
$str	vvy �
)
vv� �
,
vv� �
newww '
EnderecoConsultaPublicaNfceww /
(ww/ 0
	EstadoNfeww0 9
.ww9 :
MAww: <
,ww< =
TipoAmbienteww> J
.wwJ K

taProducaowwK U
,wwU V"
TipoUrlConsultaPublicawwW m
.wwm n
UrlConsultawwn y
,wwy z
$str	ww{ �
)
ww� �
,
ww� �
newxx '
EnderecoConsultaPublicaNfcexx /
(xx/ 0
	EstadoNfexx0 9
.xx9 :
MAxx: <
,xx< =
TipoAmbientexx> J
.xxJ K
taHomologacaoxxK X
,xxX Y"
TipoUrlConsultaPublicaxxZ p
.xxp q
	UrlQrCodexxq z
,xxz {
$str	xx| �
)
xx� �
,
xx� �
newyy '
EnderecoConsultaPublicaNfceyy /
(yy/ 0
	EstadoNfeyy0 9
.yy9 :
MAyy: <
,yy< =
TipoAmbienteyy> J
.yyJ K
taHomologacaoyyK X
,yyX Y"
TipoUrlConsultaPublicayyZ p
.yyp q
UrlConsultayyq |
,yy| }
$str	yy~ �
)
yy� �
,
yy� �
new{{ '
EnderecoConsultaPublicaNfce{{ /
({{/ 0
	EstadoNfe{{0 9
.{{9 :
MS{{: <
,{{< =
TipoAmbiente{{> J
.{{J K

taProducao{{K U
,{{U V"
TipoUrlConsultaPublica{{W m
.{{m n
	UrlQrCode{{n w
,{{w x
$str	{{y �
)
{{� �
,
{{� �
new|| '
EnderecoConsultaPublicaNfce|| /
(||/ 0
	EstadoNfe||0 9
.||9 :
MS||: <
,||< =
TipoAmbiente||> J
.||J K

taProducao||K U
,||U V"
TipoUrlConsultaPublica||W m
.||m n
UrlConsulta||n y
,||y z
$str	||{ �
)
||� �
,
||� �
new}} '
EnderecoConsultaPublicaNfce}} /
(}}/ 0
	EstadoNfe}}0 9
.}}9 :
MS}}: <
,}}< =
TipoAmbiente}}> J
.}}J K
taHomologacao}}K X
,}}X Y"
TipoUrlConsultaPublica}}Z p
.}}p q
	UrlQrCode}}q z
,}}z {
$str	}}| �
)
}}� �
,
}}� �
new~~ '
EnderecoConsultaPublicaNfce~~ /
(~~/ 0
	EstadoNfe~~0 9
.~~9 :
MS~~: <
,~~< =
TipoAmbiente~~> J
.~~J K
taHomologacao~~K X
,~~X Y"
TipoUrlConsultaPublica~~Z p
.~~p q
UrlConsulta~~q |
,~~| }
$str	~~~ �
)
~~� �
,
~~� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
MT
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
MT
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
MT
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
MT
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PA
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PA
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PA
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PA
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PB
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PB
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PB
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PB
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PI
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PI
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PI
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PI
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
PR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RJ
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RJ
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RJ
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RJ
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RN
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RN
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RN
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RN
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RO
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RO
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RO
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RO
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RR
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RS
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RS
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RS
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
RS
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SE
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SP
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
	UrlQrCode
��n w
,
��w x
$str��y �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SP
��: <
,
��< =
TipoAmbiente
��> J
.
��J K

taProducao
��K U
,
��U V$
TipoUrlConsultaPublica
��W m
.
��m n
UrlConsulta
��n y
,
��y z
$str��{ �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SP
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
	UrlQrCode
��q z
,
��z {
$str��| �
)��� �
,��� �
new
�� )
EnderecoConsultaPublicaNfce
�� /
(
��/ 0
	EstadoNfe
��0 9
.
��9 :
SP
��: <
,
��< =
TipoAmbiente
��> J
.
��J K
taHomologacao
��K X
,
��X Y$
TipoUrlConsultaPublica
��Z p
.
��p q
UrlConsulta
��q |
,
��| }
$str��~ �
)��� �
,��� �
}
�� 
;
�� 
return
�� 
endQrCodeNfce
��  
;
��  !
}
�� 	
public
�� 
static
�� 
string
�� 
ObterUrl
�� %
(
��% &
this
��& *

infNFeSupl
��+ 5

infNFeSupl
��6 @
,
��@ A
TipoAmbiente
��B N
tipoAmbiente
��O [
,
��[ \
	EstadoNfe
��] f
estado
��g m
,
��m n%
TipoUrlConsultaPublica��o �&
tipoUrlConsultaPublica��� �
)��� �
{
�� 	
var
�� 
query
�� 
=
�� 
from
�� 
qr
�� 
in
��  "
EndQrCodeNfce
��# 0
where
��1 6
qr
��7 9
.
��9 :
TipoAmbiente
��: F
==
��G I
tipoAmbiente
��J V
&&
��W Y
qr
��Z \
.
��\ ]
Estado
��] c
==
��d f
estado
��g m
&&
��n p
qr
��q s
.
��s t%
TipoUrlConsultaPublica��t �
==��� �&
tipoUrlConsultaPublica��� �
select��� �
qr��� �
.��� �
Url��� �
;��� �
var
�� 
listaRetorno
�� 
=
�� 
query
�� $
as
��% '
IList
��( -
<
��- .
string
��. 4
>
��4 5
??
��6 8
query
��9 >
.
��> ?
ToList
��? E
(
��E F
)
��F G
;
��G H
var
�� 
qtdeRetorno
�� 
=
�� 
listaRetorno
�� *
.
��* +
Count
��+ 0
(
��0 1
)
��1 2
;
��2 3
if
�� 
(
�� 
qtdeRetorno
�� 
==
�� 
$num
��  
)
��  !
throw
�� 
new
�� 
	Exception
�� #
(
��# $
string
��$ *
.
��* +
Format
��+ 1
(
��1 2
$str
��2 r
,
��r s%
tipoUrlConsultaPublica��t �
.��� �
	Descricao��� �
(��� �
)��� �
,��� �
estado��� �
,��� �
tipoAmbiente��� �
.��� �
	Descricao��� �
(��� �
)��� �
)��� �
)��� �
;��� �
if
�� 
(
�� 
qtdeRetorno
�� 
>
�� 
$num
�� 
)
��  
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ T
)
��T U
;
��U V
return
�� 
listaRetorno
�� 
.
��  
FirstOrDefault
��  .
(
��. /
)
��/ 0
;
��0 1
}
�� 	
public
�� 
static
�� 
string
�� 
ObterUrlQrCode
�� +
(
��+ ,
this
��, 0

infNFeSupl
��1 ;

infNFeSupl
��< F
,
��F G
Classes
��H O
.
��O P
NFe
��P S
nfe
��T W
,
��W X
string
��Y _
cIdToken
��` h
,
��h i
string
��j p
csc
��q t
)
��t u
{
�� 	
var
�� 
dhEmi
�� 
=
�� 
ObterHexDeString
�� (
(
��( )
nfe
��) ,
.
��, -
infNFe
��- 3
.
��3 4
ide
��4 7
.
��7 8

ProxyDhEmi
��8 B
)
��B C
;
��C D
if
�� 
(
�� 
nfe
�� 
.
�� 
	Signature
�� 
==
��  
null
��! %
)
��% &
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ e
)
��e f
;
��f g
var
�� 
digVal
�� 
=
�� 
ObterHexDeString
�� )
(
��) *
nfe
��* -
.
��- .
	Signature
��. 7
.
��7 8

SignedInfo
��8 B
.
��B C
	Reference
��C L
.
��L M
DigestValue
��M X
)
��X Y
;
��Y Z
var
�� 
cDest
�� 
=
�� 
$str
�� 
;
�� 
if
�� 
(
�� 
nfe
�� 
.
�� 
infNFe
�� 
.
�� 
dest
�� 
!=
��  "
null
��# '
)
��' (
cDest
�� 
=
�� 
$str
�� !
+
��" #
nfe
��$ '
.
��' (
infNFe
��( .
.
��. /
dest
��/ 3
.
��3 4
CPF
��4 7
+
��8 9
nfe
��: =
.
��= >
infNFe
��> D
.
��D E
dest
��E I
.
��I J
CNPJ
��J N
+
��O P
nfe
��Q T
.
��T U
infNFe
��U [
.
��[ \
dest
��\ `
.
��` a
idEstrangeiro
��a n
;
��n o
var
�� 
	dadosBase
�� 
=
�� 
$str
�� $
+
��% &
nfe
��' *
.
��* +
infNFe
��+ 1
.
��1 2
Id
��2 4
.
��4 5
	Substring
��5 >
(
��> ?
$num
��? @
)
��@ A
+
��B C
$str
��D Y
+
��Z [
(
��\ ]
(
��] ^
int
��^ a
)
��a b
nfe
��b e
.
��e f
infNFe
��f l
.
��l m
ide
��m p
.
��p q
tpAmb
��q v
)
��v w
+
��x y
cDest
��z 
+��� �
$str��� �
+��� �
dhEmi��� �
+��� �
$str��� �
+��� �
nfe
�� 
.
��  
infNFe
��  &
.
��& '
total
��' ,
.
��, -
ICMSTot
��- 4
.
��4 5
vNF
��5 8
.
��8 9
ToString
��9 A
(
��A B
$str
��B H
)
��H I
.
��I J
Replace
��J Q
(
��Q R
$char
��R U
,
��U V
$char
��W Z
)
��Z [
+
��\ ]
$str
��^ g
+
��h i
nfe
��j m
.
��m n
infNFe
��n t
.
��t u
total
��u z
.
��z {
ICMSTot��{ �
.��� �
vICMS��� �
.��� �
ToString��� �
(��� �
$str��� �
)��� �
.��� �
Replace��� �
(��� �
$char��� �
,��� �
$char��� �
)��� �
+��� �
$str��� �
+��� �
digVal��� �
+��� �
$str��� �
+��� �
cIdToken��� �
;��� �
var
�� 
dadosParaSh1
�� 
=
�� 
	dadosBase
�� (
+
��) *
csc
��+ .
;
��. /
var
�� 

sha1ComCsc
�� 
=
�� "
ObterHexSha1DeString
�� 1
(
��1 2
dadosParaSh1
��2 >
)
��> ?
;
��? @
return
�� 
ObterUrl
�� 
(
�� 

infNFeSupl
�� &
,
��& '
nfe
��( +
.
��+ ,
infNFe
��, 2
.
��2 3
ide
��3 6
.
��6 7
tpAmb
��7 <
,
��< =
nfe
��> A
.
��A B
infNFe
��B H
.
��H I
ide
��I L
.
��L M
cUF
��M P
,
��P Q$
TipoUrlConsultaPublica
��R h
.
��h i
	UrlQrCode
��i r
)
��r s
+
��t u
$str
��v y
+
��z {
	dadosBase��| �
+��� �
$str��� �
+��� �

sha1ComCsc��� �
;��� �
}
�� 	
private
�� 
static
�� 
string
�� "
ObterHexSha1DeString
�� 2
(
��2 3
string
��3 9
s
��: ;
)
��; <
{
�� 	
var
�� 
bytes
�� 
=
�� 
Encoding
��  
.
��  !
UTF8
��! %
.
��% &
GetBytes
��& .
(
��. /
s
��/ 0
)
��0 1
;
��1 2
var
�� 
sha1
�� 
=
�� 
SHA1
�� 
.
�� 
Create
�� "
(
��" #
)
��# $
;
��$ %
var
�� 
	hashBytes
�� 
=
�� 
sha1
��  
.
��  !
ComputeHash
��! ,
(
��, -
bytes
��- 2
)
��2 3
;
��3 4
return
�� !
ObterHexDeByteArray
�� &
(
��& '
	hashBytes
��' 0
)
��0 1
;
��1 2
}
�� 	
private
�� 
static
�� 
string
�� !
ObterHexDeByteArray
�� 1
(
��1 2
byte
��2 6
[
��6 7
]
��7 8
bytes
��9 >
)
��> ?
{
�� 	
var
�� 
sb
�� 
=
�� 
new
�� 
StringBuilder
�� &
(
��& '
)
��' (
;
��( )
foreach
�� 
(
�� 
var
�� 
b
�� 
in
�� 
bytes
�� #
)
��# $
{
�� 
var
�� 
hex
�� 
=
�� 
b
�� 
.
�� 
ToString
�� $
(
��$ %
$str
��% )
)
��) *
;
��* +
sb
�� 
.
�� 
Append
�� 
(
�� 
hex
�� 
)
�� 
;
�� 
}
�� 
return
�� 
sb
�� 
.
�� 
ToString
�� 
(
�� 
)
��  
;
��  !
}
�� 	
private
�� 
static
�� 
string
�� 
ObterHexDeString
�� .
(
��. /
string
��/ 5
s
��6 7
)
��7 8
{
�� 	
var
�� 
hex
�� 
=
�� 
$str
�� 
;
�� 
foreach
�� 
(
�� 
var
�� 
c
�� 
in
�� 
s
�� 
)
��  
{
�� 
int
�� 
tmp
�� 
=
�� 
c
�� 
;
�� 
hex
�� 
+=
�� 
string
�� 
.
�� 
Format
�� $
(
��$ %
$str
��% -
,
��- .
Convert
��/ 6
.
��6 7
ToUInt32
��7 ?
(
��? @
tmp
��@ C
.
��C D
ToString
��D L
(
��L M
)
��M N
)
��N O
)
��O P
;
��P Q
}
�� 
return
�� 
hex
�� 
;
�� 
}
�� 	
}
�� 
}�� �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Inutilizacao\ExtinutNFe.cs
	namespace'' 	
NFe''
 
.'' 
Utils'' 
.'' 
Inutilizacao''  
{(( 
public)) 

static)) 
class)) 

ExtinutNFe)) "
{** 
public11 
static11 
inutNFe11 
CarregarDeXmlString11 1
(111 2
this112 6
inutNFe117 >
inutNFe11? F
,11F G
string11H N
	xmlString11O X
)11X Y
{22 	
return33 

FuncoesXml33 
.33 
XmlStringParaClasse33 1
<331 2
inutNFe332 9
>339 :
(33: ;
	xmlString33; D
)33D E
;33E F
}44 	
public;; 
static;; 
string;; 
ObterXmlString;; +
(;;+ ,
this;;, 0
inutNFe;;1 8
pedInutilizacao;;9 H
);;H I
{<< 	
return== 

FuncoesXml== 
.== 
ClasseParaXmlString== 1
(==1 2
pedInutilizacao==2 A
)==A B
;==B C
}>> 	
publicFF 
staticFF 
inutNFeFF 
AssinaFF $
(FF$ %
thisFF% )
inutNFeFF* 1
inutNFeFF2 9
,FF9 :
X509Certificate2FF; K
certificadoDigitalFFL ^
)FF^ _
{GG 	
varHH 
inutNFeLocalHH 
=HH 
inutNFeHH &
;HH& '
ifII 
(II 
inutNFeLocalII 
.II 
infInutII $
.II$ %
IdII% '
==II( *
nullII+ /
)II/ 0
throwJJ 
newJJ 
	ExceptionJJ #
(JJ# $
$strJJ$ e
)JJe f
;JJf g
varLL 

assinaturaLL 
=LL 
	AssinadorLL &
.LL& '
ObterAssinaturaLL' 6
(LL6 7
inutNFeLocalLL7 C
,LLC D
inutNFeLocalLLE Q
.LLQ R
infInutLLR Y
.LLY Z
IdLLZ \
,LL\ ]
certificadoDigitalLL^ p
)LLp q
;LLq r
inutNFeLocalMM 
.MM 
	SignatureMM "
=MM# $

assinaturaMM% /
;MM/ 0
returnNN 
inutNFeLocalNN 
;NN  
}OO 	
}PP 
}QQ �	
VC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Inutilizacao\ExtretInutNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Inutilizacao%%  
{&& 
public'' 

static'' 
class'' 
ExtretInutNFe'' %
{(( 
public// 
static// 

retInutNFe//  
CarregarDeXmlString//! 4
(//4 5
this//5 9

retInutNFe//: D

retInutNFe//E O
,//O P
string//Q W
	xmlString//X a
)//a b
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2

retInutNFe112 <
>11< =
(11= >
	xmlString11> G
)11G H
;11H I
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0

retInutNFe991 ;

retInutNFe99< F
)99F G
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2

retInutNFe;;2 <
);;< =
;;;= >
}<< 	
}== 
}>> �
GC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\NfeSituacao.cs
	namespace 	
NFe
 
. 
Utils 
{ 
public 

static 
class 
NfeSituacao #
{ 
public 
static 
bool 

Autorizada %
(% &
int& )
cStat* /
)/ 0
{ 	
return 
cStat 
== 
$num 
| 
cStat 
== 
$num 
;V W
}		 	
public 
static 
bool 
	Cancelada $
($ %
int% (
cStat) .
). /
{ 	
return 
cStat 
== 
$num 
| 
cStat 
== 
$num 
| 
cStat 
== 
$num 
| 
cStat 
== 
$num 
;P Q
} 	
public 
static 
bool 
Denegada #
(# $
int$ '
cStat( -
)- .
{ 	
return 
cStat 
== 
$num 
| 
cStat 
== 
$num 
| 
cStat 
== 
$num 
| 
cStat 
== 
$num 
;[ \
} 	
public 
static 
bool 
Inutilizada &
(& '
int' *
cStat+ 0
)0 1
{ 	
return 
cStat 
== 
$num 
| 
cStat 
== 
$num 
| 
cStat 
== 
$num 
|   
cStat   
==   
$num   
;  o p
}!! 	
public## 
static## 
bool## 
	Rejeitada## $
(##$ %
int##% (
cStat##) .
)##. /
{$$ 	
return&& 
cStat&& 
>=&& 
$num&& 
&&&  !
!&&" #

Autorizada&&# -
(&&- .
cStat&&. 3
)&&3 4
&&&5 6
!&&7 8
	Cancelada&&8 A
(&&A B
cStat&&B G
)&&G H
&&&I J
!&&K L
Denegada&&L T
(&&T U
cStat&&U Z
)&&Z [
&&&\ ]
!&&^ _
Inutilizada&&_ j
(&&j k
cStat&&k p
)&&p q
;&&q r
}'' 	
public)) 
static)) 
bool)) 
LoteRecebido)) '
())' (
int))( +
cStat)), 1
)))1 2
{** 	
return++ 
cStat++ 
==++ 
$num++ 
;++  
},, 	
public.. 
static.. 
bool.. 
LoteProcessado.. )
(..) *
int..* -
cStat... 3
)..3 4
{// 	
return00 
cStat00 
==00 
$num00 
;00  
}11 	
}22 
}33 �
JC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\NFe\ExtNfeProc.cs
public%% 
static%% 
class%% 

ExtNfeProc%% 
{&& 
public-- 

static-- 
nfeProc--  
CarregarDeArquivoXml-- .
(--. /
this--/ 3
nfeProc--4 ;
nfeProc--< C
,--C D
string--E K

arquivoXml--L V
)--V W
{.. 
var// 
s// 
=// 

FuncoesXml// 
.// !
ObterNodeDeArquivoXml// 0
(//0 1
typeof//1 7
(//7 8
nfeProc//8 ?
)//? @
.//@ A
Name//A E
,//E F

arquivoXml//G Q
)//Q R
;//R S
return00 

FuncoesXml00 
.00 
XmlStringParaClasse00 -
<00- .
nfeProc00. 5
>005 6
(006 7
s007 8
)008 9
;009 :
}11 
public88 

static88 
string88 
ObterXmlString88 '
(88' (
this88( ,
nfeProc88- 4
nfeProc885 <
)88< =
{99 
return:: 

FuncoesXml:: 
.:: 
ClasseParaXmlString:: -
(::- .
nfeProc::. 5
)::5 6
;::6 7
};; 
publicCC 

staticCC 
nfeProcCC 
CarregarDeXmlStringCC -
(CC- .
thisCC. 2
nfeProcCC3 :
nfeProcCC; B
,CCB C
stringCCD J
	xmlStringCCK T
)CCT U
{DD 
varEE 
sEE 
=EE 

FuncoesXmlEE 
.EE  
ObterNodeDeStringXmlEE /
(EE/ 0
typeofEE0 6
(EE6 7
nfeProcEE7 >
)EE> ?
.EE? @
NameEE@ D
,EED E
	xmlStringEEF O
)EEO P
;EEP Q
returnFF 

FuncoesXmlFF 
.FF 
XmlStringParaClasseFF -
<FF- .
nfeProcFF. 5
>FF5 6
(FF6 7
sFF7 8
)FF8 9
;FF9 :
}GG 
publicNN 

staticNN 
voidNN 
SalvarArquivoXmlNN '
(NN' (
thisNN( ,
nfeProcNN- 4
nfeProcNN5 <
,NN< =
stringNN> D

arquivoXmlNNE O
)NNO P
{OO 

FuncoesXmlPP 
.PP  
ClasseParaArquivoXmlPP '
(PP' (
nfeProcPP( /
,PP/ 0

arquivoXmlPP1 ;
)PP; <
;PP< =
}QQ 
}RR �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Recepcao\ExtconsReciNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Recepcao%% 
{&& 
public'' 

static'' 
class'' 
ExtconsReciNFe'' &
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
consReciNFe..1 <
	pedRecibo..= F
)..F G
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
	pedRecibo002 ;
)00; <
;00< =
}11 	
}22 
}33 �
PC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Recepcao\ExtenviNFe2.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Recepcao%% 
{&& 
public'' 

static'' 
class'' 
ExtenviNFe2'' #
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
enviNFe2..1 9
pedEnvio..: B
)..B C
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
pedEnvio002 :
)00: ;
;00; <
}11 	
}22 
}33 �	
VC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Recepcao\ExtretConsReciNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Recepcao%% 
{&& 
public'' 

static'' 
class'' 
ExtretConsReciNFe'' )
{(( 
public// 
static// 
retConsReciNFe// $
CarregarDeXmlString//% 8
(//8 9
this//9 =
retConsReciNFe//> L
retConsReciNFe//M [
,//[ \
string//] c
	xmlString//d m
)//m n
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retConsReciNFe112 @
>11@ A
(11A B
	xmlString11B K
)11K L
;11L M
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retConsReciNFe991 ?
retConsReciNFe99@ N
)99N O
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retConsReciNFe;;2 @
);;@ A
;;;A B
}<< 	
}== 
}>> �	
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Recepcao\ExtretEnviNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Recepcao%% 
{&& 
public'' 

static'' 
class'' 
ExtretEnviNFe'' %
{(( 
public// 
static// 

retEnviNFe//  
CarregarDeXmlString//! 4
(//4 5
this//5 9

retEnviNFe//: D

retEnviNFe//E O
,//O P
string//Q W
	xmlString//X a
)//a b
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2

retEnviNFe112 <
>11< =
(11= >
	xmlString11> G
)11G H
;11H I
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0

retEnviNFe991 ;

retEnviNFe99< F
)99F G
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2

retEnviNFe;;2 <
);;< =
;;;= >
}<< 	
}== 
}>> �
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Status\ExtconsStatServ.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Status%% 
{&& 
public'' 

static'' 
class'' 
ExtconsStatServ'' '
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
consStatServ..1 =
	pedStatus..> G
)..G H
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
	pedStatus002 ;
)00; <
;00< =
}11 	
}22 
}33 �P
FC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\NFe\ExtNFe.cs
	namespace(( 	
NFe((
 
.(( 
Utils(( 
.(( 
NFe(( 
{)) 
public** 

static** 
class** 
ExtNFe** 
{++ 
public22 
static22 
Classes22 
.22 
NFe22 ! 
CarregarDeArquivoXml22" 6
(226 7
this227 ;
Classes22< C
.22C D
NFe22D G
nfe22H K
,22K L
string22M S

arquivoXml22T ^
)22^ _
{33 	
var44 
s44 
=44 

FuncoesXml44 
.44 !
ObterNodeDeArquivoXml44 4
(444 5
typeof445 ;
(44; <
Classes44< C
.44C D
NFe44D G
)44G H
.44H I
Name44I M
,44M N

arquivoXml44O Y
)44Y Z
;44Z [
return55 

FuncoesXml55 
.55 
XmlStringParaClasse55 1
<551 2
Classes552 9
.559 :
NFe55: =
>55= >
(55> ?
s55? @
)55@ A
;55A B
}66 	
public== 
static== 
string== 
ObterXmlString== +
(==+ ,
this==, 0
Classes==1 8
.==8 9
NFe==9 <
nfe=== @
)==@ A
{>> 	
return?? 

FuncoesXml?? 
.?? 
ClasseParaXmlString?? 1
(??1 2
nfe??2 5
)??5 6
;??6 7
}@@ 	
publicHH 
staticHH 
ClassesHH 
.HH 
NFeHH !
CarregarDeXmlStringHH" 5
(HH5 6
thisHH6 :
ClassesHH; B
.HHB C
NFeHHC F
nfeHHG J
,HHJ K
stringHHL R
	xmlStringHHS \
)HH\ ]
{II 	
varJJ 
sJJ 
=JJ 

FuncoesXmlJJ 
.JJ  
ObterNodeDeStringXmlJJ 3
(JJ3 4
typeofJJ4 :
(JJ: ;
ClassesJJ; B
.JJB C
NFeJJC F
)JJF G
.JJG H
NameJJH L
,JJL M
	xmlStringJJN W
)JJW X
;JJX Y
returnKK 

FuncoesXmlKK 
.KK 
XmlStringParaClasseKK 1
<KK1 2
ClassesKK2 9
.KK9 :
NFeKK: =
>KK= >
(KK> ?
sKK? @
)KK@ A
;KKA B
}LL 	
publicSS 
staticSS 
voidSS 
SalvarArquivoXmlSS +
(SS+ ,
thisSS, 0
ClassesSS1 8
.SS8 9
NFeSS9 <
nfeSS= @
,SS@ A
stringSSB H

arquivoXmlSSI S
)SSS T
{TT 	

FuncoesXmlUU 
.UU  
ClasseParaArquivoXmlUU +
(UU+ ,
nfeUU, /
,UU/ 0

arquivoXmlUU1 ;
)UU; <
;UU< =
}VV 	
public]] 
static]] 
Classes]] 
.]] 
NFe]] !
Valida]]" (
(]]( )
this]]) -
Classes]]. 5
.]]5 6
NFe]]6 9
nfe]]: =
)]]= >
{^^ 	
if__ 
(__ 
nfe__ 
==__ 
null__ 
)__ 
throw__ "
new__# &!
ArgumentNullException__' <
(__< =
$str__= B
)__B C
;__C D
varaa 
versaoaa 
=aa 
(aa 
Decimalaa !
.aa! "
Parseaa" '
(aa' (
nfeaa( +
.aa+ ,
infNFeaa, 2
.aa2 3
versaoaa3 9
,aa9 :
CultureInfoaa; F
.aaF G
InvariantCultureaaG W
)aaW X
)aaX Y
;aaY Z
varcc 
xmlNfecc 
=cc 
nfecc 
.cc 
ObterXmlStringcc +
(cc+ ,
)cc, -
;cc- .
vardd 

cfgServicodd 
=dd 
ConfiguracaoServicodd 0
.dd0 1
	Instanciadd1 :
;dd: ;
ifee 
(ee 
versaoee 
<ee 
$numee 
)ee 
	Validadorff 
.ff 
Validaff  
(ff  !

ServicoNFeff! +
.ff+ ,
NfeRecepcaoff, 7
,ff7 8

cfgServicoff9 C
.ffC D
VersaoNfeRecepcaoffD U
,ffU V
xmlNfeffW ]
,ff] ^
falseff_ d
)ffd e
;ffe f
ifgg 
(gg 
versaogg 
>=gg 
$numgg 
)gg 
	Validadorhh 
.hh 
Validahh  
(hh  !

ServicoNFehh! +
.hh+ ,
NFeAutorizacaohh, :
,hh: ;

cfgServicohh< F
.hhF G 
VersaoNFeAutorizacaohhG [
,hh[ \
xmlNfehh] c
,hhc d
falsehhe j
)hhj k
;hhk l
returnjj 
nfejj 
;jj 
}kk 	
publicrr 
staticrr 
Classesrr 
.rr 
NFerr !
Assinarr" (
(rr( )
thisrr) -
Classesrr. 5
.rr5 6
NFerr6 9
nferr: =
)rr= >
{ss 	
vartt 
nfeLocaltt 
=tt 
nfett 
;tt 
ifuu 
(uu 
nfeLocaluu 
==uu 
nulluu  
)uu  !
throwuu" '
newuu( +!
ArgumentNullExceptionuu, A
(uuA B
$struuB G
)uuG H
;uuH I
varyy 

tamanhocNfyy 
=yy 
$numyy 
;yy 
varzz 
versaozz 
=zz 
(zz 
decimalzz !
.zz! "
Parsezz" '
(zz' (
nfeLocalzz( 0
.zz0 1
infNFezz1 7
.zz7 8
versaozz8 >
,zz> ?
CultureInfozz@ K
.zzK L
InvariantCulturezzL \
)zz\ ]
)zz] ^
;zz^ _
if{{ 
({{ 
versao{{ 
>={{ 
$num{{ 
){{ 

tamanhocNf{{ '
={{( )
$num{{* +
;{{+ ,
nfeLocal|| 
.|| 
infNFe|| 
.|| 
ide|| 
.||  
cNF||  #
=||$ %
Convert||& -
.||- .
ToInt32||. 5
(||5 6
nfeLocal||6 >
.||> ?
infNFe||? E
.||E F
ide||F I
.||I J
cNF||J M
)||M N
.||N O
ToString||O W
(||W X
)||X Y
.||Y Z
PadLeft||Z a
(||a b

tamanhocNf||b l
,||l m
$char||n q
)||q r
;||r s
var
�� #
modeloDocumentoFiscal
�� %
=
��& '
nfeLocal
��( 0
.
��0 1
infNFe
��1 7
.
��7 8
ide
��8 ;
.
��; <
mod
��< ?
;
��? @
var
�� 
tipoEmissao
�� 
=
�� 
(
�� 
int
�� "
)
��" #
nfeLocal
��# +
.
��+ ,
infNFe
��, 2
.
��2 3
ide
��3 6
.
��6 7
tpEmis
��7 =
;
��= >
var
�� 
codigoNumerico
�� 
=
��  
int
��! $
.
��$ %
Parse
��% *
(
��* +
nfeLocal
��+ 3
.
��3 4
infNFe
��4 :
.
��: ;
ide
��; >
.
��> ?
cNF
��? B
)
��B C
;
��C D
var
�� 
estado
�� 
=
�� 
nfeLocal
�� !
.
��! "
infNFe
��" (
.
��( )
ide
��) ,
.
��, -
cUF
��- 0
;
��0 1
var
�� 
dataEHoraEmissao
��  
=
��! "
nfeLocal
��# +
.
��+ ,
infNFe
��, 2
.
��2 3
ide
��3 6
.
��6 7
dhEmi
��7 <
;
��< =
var
�� 
cnpj
�� 
=
�� 
nfeLocal
�� 
.
��  
infNFe
��  &
.
��& '
emit
��' +
.
��+ ,
CNPJ
��, 0
;
��0 1
var
�� 
numeroDocumento
�� 
=
��  !
nfeLocal
��" *
.
��* +
infNFe
��+ 1
.
��1 2
ide
��2 5
.
��5 6
nNF
��6 9
;
��9 :
var
�� 
serie
�� 
=
�� 
nfeLocal
��  
.
��  !
infNFe
��! '
.
��' (
ide
��( +
.
��+ ,
serie
��, 1
;
��1 2
var
�� 

dadosChave
�� 
=
�� 
ChaveFiscal
�� (
.
��( )

ObterChave
��) 3
(
��3 4
estado
��4 :
,
��: ;
dataEHoraEmissao
��< L
,
��L M
cnpj
��N R
,
��R S#
modeloDocumentoFiscal
��T i
,
��i j
serie
��k p
,
��p q
numeroDocumento��r �
,��� �
tipoEmissao��� �
,��� �
codigoNumerico��� �
)��� �
;��� �
nfeLocal
�� 
.
�� 
infNFe
�� 
.
�� 
Id
�� 
=
��  
$str
��! &
+
��' (

dadosChave
��) 3
.
��3 4
Chave
��4 9
;
��9 :
nfeLocal
�� 
.
�� 
infNFe
�� 
.
�� 
ide
�� 
.
��  
cDV
��  #
=
��$ %
Convert
��& -
.
��- .
ToInt16
��. 5
(
��5 6

dadosChave
��6 @
.
��@ A
DigitoVerificador
��A R
)
��R S
;
��S T
var
�� 

assinatura
�� 
=
�� 
	Assinador
�� &
.
��& '
ObterAssinatura
��' 6
(
��6 7
nfeLocal
��7 ?
,
��? @
nfeLocal
��A I
.
��I J
infNFe
��J P
.
��P Q
Id
��Q S
)
��S T
;
��T U
nfeLocal
�� 
.
�� 
	Signature
�� 
=
��  

assinatura
��! +
;
��+ ,
return
�� 
nfeLocal
�� 
;
�� 
}
�� 	
}
�� 
}�� �
UC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Consulta\ExtprocEventoNFe.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Consulta%% 
{&& 
public'' 

static'' 
class'' 
ExtprocEventoNFe'' (
{(( 
public.. 
static.. 
string.. 
ObterXmlString.. +
(..+ ,
this.., 0
procEventoNFe..1 >
procEventoNFe..? L
)..L M
{// 	
return00 

FuncoesXml00 
.00 
ClasseParaXmlString00 1
(001 2
procEventoNFe002 ?
)00? @
;00@ A
}11 	
public99 
static99 
procEventoNFe99 #
CarregarDeXmlString99$ 7
(997 8
this998 <
procEventoNFe99= J
procEventoNFe99K X
,99X Y
string99Z `
	xmlString99a j
)99j k
{:: 	
var;; 
s;; 
=;; 

FuncoesXml;; 
.;;  
ObterNodeDeStringXml;; 3
(;;3 4
typeof;;4 :
(;;: ;
procEventoNFe;;; H
);;H I
.;;I J
Name;;J N
,;;N O
	xmlString;;P Y
);;Y Z
;;;Z [
return<< 

FuncoesXml<< 
.<< 
XmlStringParaClasse<< 1
<<<1 2
procEventoNFe<<2 ?
><<? @
(<<@ A
s<<A B
)<<B C
;<<C D
}== 	
}>> 
}?? �	
UC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Status\ExtretConsStatServ.cs
	namespace%% 	
NFe%%
 
.%% 
Utils%% 
.%% 
Status%% 
{&& 
public'' 

static'' 
class'' 
ExtretConsStatServ'' *
{(( 
public// 
static// 
retConsStatServ// %
CarregarDeXmlString//& 9
(//9 :
this//: >
retConsStatServ//? N
retConsStatServ//O ^
,//^ _
string//` f
	xmlString//g p
)//p q
{00 	
return11 

FuncoesXml11 
.11 
XmlStringParaClasse11 1
<111 2
retConsStatServ112 A
>11A B
(11B C
	xmlString11C L
)11L M
;11M N
}22 	
public99 
static99 
string99 
ObterXmlString99 +
(99+ ,
this99, 0
retConsStatServ991 @
retConsStatServ99A P
)99P Q
{:: 	
return;; 

FuncoesXml;; 
.;; 
ClasseParaXmlString;; 1
(;;1 2
retConsStatServ;;2 A
);;A B
;;;B C
}<< 	
}== 
}>> �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Properties\AssemblyInfo.cs
[(( 
assembly(( 	
:((	 

AssemblyTitle(( 
((( 
$str(( $
)(($ %
]((% &
[)) 
assembly)) 	
:))	 

AssemblyDescription)) 
()) 
$str)) !
)))! "
]))" #
[** 
assembly** 	
:**	 
!
AssemblyConfiguration**  
(**  !
$str**! #
)**# $
]**$ %
[++ 
assembly++ 	
:++	 

AssemblyCompany++ 
(++ 
$str++ 
)++ 
]++ 
[,, 
assembly,, 	
:,,	 

AssemblyProduct,, 
(,, 
$str,, &
),,& '
],,' (
[-- 
assembly-- 	
:--	 

AssemblyCopyright-- 
(-- 
$str-- 0
)--0 1
]--1 2
[.. 
assembly.. 	
:..	 

AssemblyTrademark.. 
(.. 
$str.. 
)..  
]..  !
[// 
assembly// 	
://	 

AssemblyCulture// 
(// 
$str// 
)// 
]// 
[55 
assembly55 	
:55	 


ComVisible55 
(55 
false55 
)55 
]55 
[99 
assembly99 	
:99	 

Guid99 
(99 
$str99 6
)996 7
]997 8
[FF 
assemblyFF 	
:FF	 

AssemblyVersionFF 
(FF 
$strFF &
)FF& '
]FF' (
[GG 
assemblyGG 	
:GG	 

AssemblyFileVersionGG 
(GG 
$strGG *
)GG* +
]GG+ ,�`
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Tributacao\Estadual\ICMSGeral.cs
	namespace(( 	
NFe((
 
.(( 
Utils(( 
.(( 

Tributacao(( 
.(( 
Estadual(( '
{)) 
public-- 

class-- 
	ICMSGeral-- 
{.. 
public33 
	ICMSGeral33 
(33 

ICMSBasico33 #

icmsBasico33$ .
)33. /
{44 	
this55 
.55 
CopiarPropriedades55 #
(55# $

icmsBasico55$ .
)55. /
;55/ 0
}66 	
public88 
	ICMSGeral88 
(88 
)88 
{99 	
};; 	
publicCC 

ICMSBasicoCC 
ObterICMSBasicoCC )
(CC) *
CRTCC* -
crtCC. 1
)CC1 2
{DD 	

ICMSBasicoEE 

icmsBasicoEE !
;EE! "
switchGG 
(GG 
crtGG 
)GG 
{HH 
caseII 
CRTII 
.II 
SimplesNacionalII (
:II( )
switchJJ 
(JJ 
CSOSNJJ !
)JJ! "
{KK 
caseLL 
	CsosnicmsLL &
.LL& '
Csosn101LL' /
:LL/ 0

icmsBasicoMM &
=MM' (
newMM) ,
	ICMSSN101MM- 6
(MM6 7
)MM7 8
;MM8 9
breakNN !
;NN! "
caseOO 
	CsosnicmsOO &
.OO& '
Csosn102OO' /
:OO/ 0
casePP 
	CsosnicmsPP &
.PP& '
Csosn103PP' /
:PP/ 0
caseQQ 
	CsosnicmsQQ &
.QQ& '
Csosn300QQ' /
:QQ/ 0
caseRR 
	CsosnicmsRR &
.RR& '
Csosn400RR' /
:RR/ 0

icmsBasicoSS &
=SS' (
newSS) ,
	ICMSSN102SS- 6
(SS6 7
)SS7 8
;SS8 9
breakTT !
;TT! "
caseUU 
	CsosnicmsUU &
.UU& '
Csosn201UU' /
:UU/ 0

icmsBasicoVV &
=VV' (
newVV) ,
	ICMSSN201VV- 6
(VV6 7
)VV7 8
;VV8 9
breakWW !
;WW! "
caseXX 
	CsosnicmsXX &
.XX& '
Csosn202XX' /
:XX/ 0
caseYY 
	CsosnicmsYY &
.YY& '
Csosn203YY' /
:YY/ 0

icmsBasicoZZ &
=ZZ' (
newZZ) ,
	ICMSSN202ZZ- 6
(ZZ6 7
)ZZ7 8
;ZZ8 9
break[[ !
;[[! "
case\\ 
	Csosnicms\\ &
.\\& '
Csosn500\\' /
:\\/ 0

icmsBasico]] &
=]]' (
new]]) ,
	ICMSSN500]]- 6
(]]6 7
)]]7 8
;]]8 9
break^^ !
;^^! "
case__ 
	Csosnicms__ &
.__& '
Csosn900__' /
:__/ 0

icmsBasico`` &
=``' (
new``) ,
	ICMSSN900``- 6
(``6 7
)``7 8
;``8 9
breakaa !
;aa! "
defaultbb 
:bb  
throwcc !
newcc" %'
ArgumentOutOfRangeExceptioncc& A
(ccA B
)ccB C
;ccC D
}dd 
breakee 
;ee 
caseff 
CRTff 
.ff +
SimplesNacionalExcessoSublimiteff 8
:ff8 9
casegg 
CRTgg 
.gg 
RegimeNormalgg %
:gg% &
switchhh 
(hh 
CSThh 
)hh  
{ii 
casejj 
Csticmsjj $
.jj$ %
Cst00jj% *
:jj* +

icmsBasicokk &
=kk' (
newkk) ,
ICMS00kk- 3
(kk3 4
)kk4 5
;kk5 6
breakll !
;ll! "
casemm 
Csticmsmm $
.mm$ %
Cst10mm% *
:mm* +

icmsBasiconn &
=nn' (
newnn) ,
ICMS10nn- 3
(nn3 4
)nn4 5
;nn5 6
breakoo !
;oo! "
casepp 
Csticmspp $
.pp$ %
	CstPart10pp% .
:pp. /
caseqq 
Csticmsqq $
.qq$ %
	CstPart90qq% .
:qq. /

icmsBasicorr &
=rr' (
newrr) ,
ICMSPartrr- 5
(rr5 6
)rr6 7
;rr7 8
breakss !
;ss! "
casett 
Csticmstt $
.tt$ %
Cst20tt% *
:tt* +

icmsBasicouu &
=uu' (
newuu) ,
ICMS20uu- 3
(uu3 4
)uu4 5
;uu5 6
breakvv !
;vv! "
caseww 
Csticmsww $
.ww$ %
Cst30ww% *
:ww* +

icmsBasicoxx &
=xx' (
newxx) ,
ICMS30xx- 3
(xx3 4
)xx4 5
;xx5 6
breakyy !
;yy! "
casezz 
Csticmszz $
.zz$ %
Cst40zz% *
:zz* +
case{{ 
Csticms{{ $
.{{$ %
Cst41{{% *
:{{* +
case|| 
Csticms|| $
.||$ %
Cst50||% *
:||* +

icmsBasico}} &
=}}' (
new}}) ,
ICMS40}}- 3
(}}3 4
)}}4 5
;}}5 6
break~~ !
;~~! "
case 
Csticms $
.$ %
CstRep41% -
:- .

icmsBasico
�� &
=
��' (
new
��) ,
ICMSST
��- 3
(
��3 4
)
��4 5
;
��5 6
break
�� !
;
��! "
case
�� 
Csticms
�� $
.
��$ %
Cst51
��% *
:
��* +

icmsBasico
�� &
=
��' (
new
��) ,
ICMS51
��- 3
(
��3 4
)
��4 5
;
��5 6
break
�� !
;
��! "
case
�� 
Csticms
�� $
.
��$ %
Cst60
��% *
:
��* +

icmsBasico
�� &
=
��' (
new
��) ,
ICMS60
��- 3
(
��3 4
)
��4 5
;
��5 6
break
�� !
;
��! "
case
�� 
Csticms
�� $
.
��$ %
Cst70
��% *
:
��* +

icmsBasico
�� &
=
��' (
new
��) ,
ICMS70
��- 3
(
��3 4
)
��4 5
;
��5 6
break
�� !
;
��! "
case
�� 
Csticms
�� $
.
��$ %
Cst90
��% *
:
��* +

icmsBasico
�� &
=
��' (
new
��) ,
ICMS90
��- 3
(
��3 4
)
��4 5
;
��5 6
break
�� !
;
��! "
default
�� 
:
��  
throw
�� !
new
��" %)
ArgumentOutOfRangeException
��& A
(
��A B
)
��B C
;
��C D
}
�� 
break
�� 
;
�� 
default
�� 
:
�� 
throw
�� 
new
�� )
ArgumentOutOfRangeException
�� 9
(
��9 :
$str
��: ?
,
��? @
crt
��A D
,
��D E
null
��F J
)
��J K
;
��K L
}
�� 

icmsBasico
�� 
.
��  
CopiarPropriedades
�� )
(
��) *
this
��* .
)
��. /
;
��/ 0
return
�� 

icmsBasico
�� 
;
�� 
}
�� 	
public
�� 
OrigemMercadoria
�� 
orig
��  $
{
��% &
get
��' *
;
��* +
set
��, /
;
��/ 0
}
��1 2
public
�� 
Csticms
�� 
CST
�� 
{
�� 
get
��  
;
��  !
set
��" %
;
��% &
}
��' (
public
�� "
DeterminacaoBaseIcms
�� #
modBC
��$ )
{
��* +
get
��, /
;
��/ 0
set
��1 4
;
��4 5
}
��6 7
public
�� 
decimal
�� 
vBC
�� 
{
�� 
get
��  
;
��  !
set
��" %
;
��% &
}
��' (
public
�� 
decimal
�� 
pICMS
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
public
�� 
decimal
�� 
vICMS
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
public
�� $
DeterminacaoBaseIcmsSt
�� %
modBCST
��& -
{
��. /
get
��0 3
;
��3 4
set
��5 8
;
��8 9
}
��: ;
public
�� 
decimal
�� 
?
�� 
pMVAST
�� 
{
��  
get
��! $
;
��$ %
set
��& )
;
��) *
}
��+ ,
public
�� 
decimal
�� 
?
�� 
pRedBCST
��  
{
��! "
get
��# &
;
��& '
set
��( +
;
��+ ,
}
��- .
public
�� 
decimal
�� 
vBCST
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
public
�� 
decimal
�� 
pICMSST
�� 
{
��  
get
��! $
;
��$ %
set
��& )
;
��) *
}
��+ ,
public
�� 
decimal
�� 
vICMSST
�� 
{
��  
get
��! $
;
��$ %
set
��& )
;
��) *
}
��+ ,
public
�� 
decimal
�� 
pRedBC
�� 
{
�� 
get
��  #
;
��# $
set
��% (
;
��( )
}
��* +
public
�� 
decimal
�� 
?
�� 

vICMSDeson
�� "
{
��# $
get
��% (
;
��( )
set
��* -
;
��- .
}
��/ 0
public
�� #
MotivoDesoneracaoIcms
�� $
?
��$ %

motDesICMS
��& 0
{
��1 2
get
��3 6
;
��6 7
set
��8 ;
;
��; <
}
��= >
public
�� 
decimal
�� 
?
�� 
vICMSOp
�� 
{
��  !
get
��" %
;
��% &
set
��' *
;
��* +
}
��, -
public
�� 
decimal
�� 
?
�� 
pDif
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
public
�� 
decimal
�� 
?
�� 
vICMSDif
��  
{
��! "
get
��# &
;
��& '
set
��( +
;
��+ ,
}
��- .
public
�� 
decimal
�� 
?
�� 
vBCSTRet
��  
{
��! "
get
��# &
;
��& '
set
��( +
;
��+ ,
}
��- .
public
�� 
decimal
�� 
?
�� 

vICMSSTRet
�� "
{
��# $
get
��% (
;
��( )
set
��* -
;
��- .
}
��/ 0
public
�� 
decimal
�� 
pBCOp
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
public
�� 
string
�� 
UFST
�� 
{
�� 
get
��  
;
��  !
set
��" %
;
��% &
}
��' (
public
�� 
decimal
�� 
	vBCSTDest
��  
{
��! "
get
��# &
;
��& '
set
��( +
;
��+ ,
}
��- .
public
�� 
decimal
�� 
vICMSSTDest
�� "
{
��# $
get
��% (
;
��( )
set
��* -
;
��- .
}
��/ 0
public
�� 
	Csosnicms
�� 
CSOSN
�� 
{
��  
get
��! $
;
��$ %
set
��& )
;
��) *
}
��+ ,
public
�� 
decimal
�� 
pCredSN
�� 
{
��  
get
��! $
;
��$ %
set
��& )
;
��) *
}
��+ ,
public
�� 
decimal
�� 
vCredICMSSN
�� "
{
��# $
get
��% (
;
��( )
set
��* -
;
��- .
}
��/ 0
}
�� 
}�� �;
OC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Utils\Validacao\Validador.cs
	namespace(( 	
NFe((
 
.(( 
Utils(( 
.(( 
	Validacao(( 
{)) 
public** 

static** 
class** 
	Validador** !
{++ 
internal,, 
static,, 
string,, 
ObterArquivoSchema,, 1
(,,1 2

ServicoNFe,,2 <

servicoNFe,,= G
,,,G H
VersaoServico,,I V
versaoServico,,W d
,,,d e
bool,,f j
loteNfe,,k r
=,,s t
true,,u y
),,y z
{-- 	
switch.. 
(.. 

servicoNFe.. 
).. 
{// 
case00 

ServicoNFe00 
.00  
NfeRecepcao00  +
:00+ ,
return11 
loteNfe11 "
?11# $
$str11% 8
:119 :
$str11; J
;11J K
case22 

ServicoNFe22 
.22  %
RecepcaoEventoCancelmento22  9
:229 :
return33 
$str33 7
;337 8
case44 

ServicoNFe44 
.44  '
RecepcaoEventoCartaCorrecao44  ;
:44; <
return55 
$str55 -
;55- .
case66 

ServicoNFe66 
.66  
RecepcaoEventoEpec66  2
:662 3
return77 
$str77 .
;77. /
case88 

ServicoNFe88 
.88  2
&RecepcaoEventoManifestacaoDestinatario88  F
:88F G
return99 
$str99 5
;995 6
case:: 

ServicoNFe:: 
.::  
NfeInutilizacao::  /
:::/ 0
switch;; 
(;; 
versaoServico;; )
);;) *
{<< 
case== 
VersaoServico== *
.==* +
ve200==+ 0
:==0 1
return>> "
$str>># 6
;>>6 7
case?? 
VersaoServico?? *
.??* +
ve310??+ 0
:??0 1
return@@ "
$str@@# 6
;@@6 7
}AA 
breakBB 
;BB 
caseCC 

ServicoNFeCC 
.CC   
NfeConsultaProtocoloCC  4
:CC4 5
switchDD 
(DD 
versaoServicoDD )
)DD) *
{EE 
caseFF 
VersaoServicoFF *
.FF* +
ve200FF+ 0
:FF0 1
returnGG "
$strGG# 9
;GG9 :
caseHH 
VersaoServicoHH *
.HH* +
ve310HH+ 0
:HH0 1
returnII "
$strII# 9
;II9 :
}JJ 
breakKK 
;KK 
caseLL 

ServicoNFeLL 
.LL  
NfeStatusServicoLL  0
:LL0 1
switchMM 
(MM 
versaoServicoMM )
)MM) *
{NN 
caseOO 
VersaoServicoOO *
.OO* +
ve200OO+ 0
:OO0 1
returnPP "
$strPP# ;
;PP; <
caseQQ 
VersaoServicoQQ *
.QQ* +
ve310QQ+ 0
:QQ0 1
returnRR "
$strRR# ;
;RR; <
}SS 
breakTT 
;TT 
caseUU 

ServicoNFeUU 
.UU  
NFeAutorizacaoUU  .
:UU. /
returnVV 
loteNfeVV "
?VV# $
$strVV% 8
:VV9 :
$strVV; J
;VVJ K
caseWW 

ServicoNFeWW 
.WW  
NfeConsultaCadastroWW  3
:WW3 4
returnXX 
$strXX .
;XX. /
caseYY 

ServicoNFeYY 
.YY  
NfeDownloadNFYY  -
:YY- .
returnZZ 
$strZZ 2
;ZZ2 3
case[[ 

ServicoNFe[[ 
.[[  
NFeDistribuicaoDFe[[  2
:[[2 3
return\\ 
$str\\ 1
;\\1 2
}]] 
return^^ 
null^^ 
;^^ 
}__ 	
publicaa 
staticaa 
voidaa 
Validaaa !
(aa! "

ServicoNFeaa" ,

servicoNFeaa- 7
,aa7 8
VersaoServicoaa9 F
versaoServicoaaG T
,aaT U
stringaaV \
	stringXmlaa] f
,aaf g
boolaah l
loteNfeaam t
=aau v
trueaaw {
)aa{ |
{bb 	
varcc 

pathSchemacc 
=cc 
ConfiguracaoServicocc 0
.cc0 1
	Instanciacc1 :
.cc: ;
DiretorioSchemascc; K
;ccK L
ifee 
(ee 
!ee 
	Directoryee 
.ee 
Existsee !
(ee! "

pathSchemaee" ,
)ee, -
)ee- .
throwff 
newff 
	Exceptionff #
(ff# $
$strff$ M
+ffN O

pathSchemaffP Z
)ffZ [
;ff[ \
varhh 
arquivoSchemahh 
=hh 

pathSchemahh  *
+hh+ ,
$strhh- 1
+hh2 3
ObterArquivoSchemahh4 F
(hhF G

servicoNFehhG Q
,hhQ R
versaoServicohhS `
,hh` a
loteNfehhb i
)hhi j
;hhj k
varkk 
cfgkk 
=kk 
newkk 
XmlReaderSettingskk +
{kk, -
ValidationTypekk. <
=kk= >
ValidationTypekk? M
.kkM N
SchemakkN T
}kkU V
;kkV W
varnn 
schemasnn 
=nn 
newnn 
XmlSchemaSetnn *
(nn* +
)nn+ ,
;nn, -
cfgoo 
.oo 
Schemasoo 
=oo 
schemasoo !
;oo! "
schemasrr 
.rr 
Addrr 
(rr 
nullrr 
,rr 
arquivoSchemarr +
)rr+ ,
;rr, -
cfgtt 
.tt "
ValidationEventHandlertt &
+=tt' )"
ValidationEventHandlertt* @
;tt@ A
varvv 
	validatorvv 
=vv 
	XmlReadervv %
.vv% &
Createvv& ,
(vv, -
newvv- 0
StringReadervv1 =
(vv= >
	stringXmlvv> G
)vvG H
,vvH I
cfgvvJ M
)vvM N
;vvN O
tryww 
{xx 
whilezz 
(zz 
	validatorzz  
.zz  !
Readzz! %
(zz% &
)zz& '
)zz' (
{{{ 
}|| 
}}} 
catch~~ 
(~~ 
XmlException~~ 
err~~  #
)~~# $
{ 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ V
+
��W X
$str
��Y ]
+
��^ _
err
��` c
.
��c d
Message
��d k
)
��k l
;
��l m
}
�� 
finally
�� 
{
�� 
	validator
�� 
.
�� 
Close
�� 
(
��  
)
��  !
;
��! "
}
�� 
}
�� 	
internal
�� 
static
�� 
void
�� $
ValidationEventHandler
�� 3
(
��3 4
object
��4 :
sender
��; A
,
��A B!
ValidationEventArgs
��C V
args
��W [
)
��[ \
{
�� 	
throw
�� 
new
�� &
ValidacaoSchemaException
�� .
(
��. /
args
��/ 3
.
��3 4
Message
��4 ;
)
��; <
;
��< =
}
�� 	
}
�� 
}�� 
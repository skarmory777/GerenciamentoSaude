�
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\Assinatura\AssinaturaDigital.cs
	namespace'' 	
DFe''
 
.'' 
Utils'' 
.'' 

Assinatura'' 
{(( 
public)) 

class)) 
AssinaturaDigital)) "
{** 
public++ 
static++ 
SignatureZeus++ #
Assina++$ *
<++* +
T+++ ,
>++, -
(++- .
T++. /
objeto++0 6
,++6 7
string++8 >
id++? A
,++A B
X509Certificate2++C S
certificado++T _
)++_ `
where++a f
T++g h
:++i j
class++k p
{,, 	
var-- 
objetoLocal-- 
=-- 
objeto-- $
;--$ %
if.. 
(.. 
id.. 
==.. 
null.. 
).. 
throw// 
new// 
	Exception// #
(//# $
$str//$ d
)//d e
;//e f
var11 
	documento11 
=11 
new11 
XmlDocument11  +
{11, -
PreserveWhitespace11. @
=11A B
true11C G
}11H I
;11I J
	documento22 
.22 
LoadXml22 
(22 

FuncoesXml22 (
.22( )
ClasseParaXmlString22) <
(22< =
objetoLocal22= H
)22H I
)22I J
;22J K
var33 
docXml33 
=33 
new33 
	SignedXml33 &
(33& '
	documento33' 0
)330 1
{332 3

SigningKey334 >
=33? @
certificado33A L
.33L M

PrivateKey33M W
}33X Y
;33Y Z
var44 
	reference44 
=44 
new44 
	Reference44  )
{44* +
Uri44, /
=440 1
$str442 5
+446 7
id448 :
}44; <
;44< =
var77 
envelopedSigntature77 #
=77$ %
new77& ).
"XmlDsigEnvelopedSignatureTransform77* L
(77L M
)77M N
;77N O
	reference88 
.88 
AddTransform88 "
(88" #
envelopedSigntature88# 6
)886 7
;887 8
var:: 
c14Transform:: 
=:: 
new:: " 
XmlDsigC14NTransform::# 7
(::7 8
)::8 9
;::9 :
	reference;; 
.;; 
AddTransform;; "
(;;" #
c14Transform;;# /
);;/ 0
;;;0 1
docXml== 
.== 
AddReference== 
(==  
	reference==  )
)==) *
;==* +
var@@ 
keyInfo@@ 
=@@ 
new@@ 
KeyInfo@@ %
(@@% &
)@@& '
;@@' (
keyInfoAA 
.AA 
	AddClauseAA 
(AA 
newAA !
KeyInfoX509DataAA" 1
(AA1 2
certificadoAA2 =
)AA= >
)AA> ?
;AA? @
docXmlCC 
.CC 
KeyInfoCC 
=CC 
keyInfoCC $
;CC$ %
docXmlDD 
.DD 
ComputeSignatureDD #
(DD# $
)DD$ %
;DD% &
varGG 
xmlDigitalSignatureGG #
=GG$ %
docXmlGG& ,
.GG, -
GetXmlGG- 3
(GG3 4
)GG4 5
;GG5 6
varHH 

assinaturaHH 
=HH 

FuncoesXmlHH '
.HH' (
XmlStringParaClasseHH( ;
<HH; <
ClassesHH< C
.HHC D

AssinaturaHHD N
.HHN O
	SignatureHHO X
>HHX Y
(HHY Z
xmlDigitalSignatureHHZ m
.HHm n
OuterXmlHHn v
)HHv w
;HHw x
returnII 

assinaturaII 
;II 
}JJ 	
}KK 
}LL ă
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\Assinatura\CertificadoDigital.cs
	namespace)) 	
DFe))
 
.)) 
Utils)) 
.)) 

Assinatura)) 
{** 
public++ 

static++ 
class++ 
CertificadoDigital++ *
{,, 
private-- 
static-- 
X509Certificate2-- '
_certificado--( 4
;--4 5
private66 
static66 
	X509Store66  
ObterX509Store66! /
(66/ 0
	OpenFlags660 9
	openFlags66: C
)66C D
{77 	
var88 
store88 
=88 
new88 
	X509Store88 %
(88% &
	StoreName88& /
.88/ 0
My880 2
,882 3
StoreLocation884 A
.88A B
CurrentUser88B M
)88M N
;88N O
store99 
.99 
Open99 
(99 
	openFlags99  
)99  !
;99! "
return:: 
store:: 
;:: 
};; 	
privateEE 
staticEE 
X509Certificate2EE '
ObterDeArquivoEE( 6
(EE6 7
stringEE7 =
arquivoEE> E
,EEE F
stringEEG M
senhaEEN S
)EES T
{FF 	
ifGG 
(GG 
!GG 
FileGG 
.GG 
ExistsGG 
(GG 
arquivoGG $
)GG$ %
)GG% &
{HH 
throwII 
newII 
	ExceptionII #
(II# $
StringII$ *
.II* +
FormatII+ 1
(II1 2
$strII2 [
,II[ \
arquivoII] d
)IId e
)IIe f
;IIf g
}JJ 
varLL 
certificadoLL 
=LL 
newLL !
X509Certificate2LL" 2
(LL2 3
arquivoLL3 :
,LL: ;
senhaLL< A
,LLA B
X509KeyStorageFlagsLLC V
.LLV W
MachineKeySetLLW d
)LLd e
;LLe f
returnMM 
certificadoMM 
;MM 
}NN 	
privateTT 
staticTT 
X509Certificate2TT '
ObterDoRepositorioTT( :
(TT: ;
stringTT; A
serialTTB H
,TTH I
	OpenFlagsTTJ S
opcoesDeAberturaTTT d
)TTd e
{UU 	
X509Certificate2VV 
certificadoVV (
=VV) *
nullVV+ /
;VV/ 0
varWW 
storeWW 
=WW 
ObterX509StoreWW &
(WW& '
opcoesDeAberturaWW' 7
)WW7 8
;WW8 9
tryXX 
{YY 
foreachZZ 
(ZZ 
varZZ 
itemZZ !
inZZ" $
storeZZ% *
.ZZ* +
CertificatesZZ+ 7
)ZZ7 8
{[[ 
if\\ 
(\\ 
item\\ 
.\\ 
SerialNumber\\ )
!=\\* ,
null\\- 1
&&\\2 4
item\\5 9
.\\9 :
SerialNumber\\: F
.\\F G
ToUpper\\G N
(\\N O
)\\O P
.\\P Q
Equals\\Q W
(\\W X
serial\\X ^
.\\^ _
ToUpper\\_ f
(\\f g
)\\g h
,\\h i
StringComparison\\j z
.\\z {'
InvariantCultureIgnoreCase	\\{ �
)
\\� �
)
\\� �
certificado]] #
=]]$ %
item]]& *
;]]* +
}^^ 
if`` 
(`` 
certificado`` 
==``  "
null``# '
)``' (
throwaa 
newaa 
	Exceptionaa '
(aa' (
stringaa( .
.aa. /
Formataa/ 5
(aa5 6
$straa6 b
,aab c
serialaad j
.aaj k
ToUpperaak r
(aar s
)aas t
)aat u
)aau v
;aav w
}bb 
finallycc 
{dd 
storeee 
.ee 
Closeee 
(ee 
)ee 
;ee 
}ff 
returnhh 
certificadohh 
;hh 
}ii 	
privateqq 
staticqq 
X509Certificate2qq ')
ObterDoRepositorioPassandoPinqq( E
(qqE F
stringqqF L
serialqqM S
,qqS T
stringqqU [
senhaqq\ a
=qqb c
nullqqd h
)qqh i
{rr 	
varss 
certificadoss 
=ss 
ObterDoRepositorioss 0
(ss0 1
serialss1 7
,ss7 8
	OpenFlagsss9 B
.ssB C
ReadOnlyssC K
)ssK L
;ssL M
iftt 
(tt 
stringtt 
.tt 
IsNullOrEmptytt $
(tt$ %
senhatt% *
)tt* +
)tt+ ,
returntt- 3
certificadott4 ?
;tt? @
certificadouu 
.uu &
DefinirPinParaChavePrivadauu 2
(uu2 3
senhauu3 8
)uu8 9
;uu9 :
returnvv 
certificadovv 
;vv 
}ww 	
private~~ 
static~~ 
void~~ &
DefinirPinParaChavePrivada~~ 6
(~~6 7
this~~7 ;
X509Certificate2~~< L
certificado~~M X
,~~X Y
string~~Z `
pin~~a d
)~~d e
{ 	
if
�� 
(
�� 
certificado
�� 
==
�� 
null
�� #
)
��# $
throw
��% *
new
��+ .#
ArgumentNullException
��/ D
(
��D E
$str
��E R
)
��R S
;
��S T
var
�� 
key
�� 
=
�� 
(
�� &
RSACryptoServiceProvider
�� /
)
��/ 0
certificado
��0 ;
.
��; <

PrivateKey
��< F
;
��F G
var
�� 
providerHandle
�� 
=
��  
IntPtr
��! '
.
��' (
Zero
��( ,
;
��, -
var
�� 
	pinBuffer
�� 
=
�� 
Encoding
�� $
.
��$ %
ASCII
��% *
.
��* +
GetBytes
��+ 3
(
��3 4
pin
��4 7
)
��7 8
;
��8 9
MetodosNativos
�� 
.
�� 
Executar
�� #
(
��# $
(
��$ %
)
��% &
=>
��' )
MetodosNativos
��* 8
.
��8 9!
CryptAcquireContext
��9 L
(
��L M
ref
��M P
providerHandle
��Q _
,
��_ `
key
�� 
.
�� !
CspKeyContainerInfo
�� '
.
��' (
KeyContainerName
��( 8
,
��8 9
key
�� 
.
�� !
CspKeyContainerInfo
�� '
.
��' (
ProviderName
��( 4
,
��4 5
key
�� 
.
�� !
CspKeyContainerInfo
�� '
.
��' (
ProviderType
��( 4
,
��4 5
MetodosNativos
�� 
.
�� 
CryptContextFlags
�� 0
.
��0 1
Silent
��1 7
)
��7 8
)
��8 9
;
��9 :
MetodosNativos
�� 
.
�� 
Executar
�� #
(
��# $
(
��$ %
)
��% &
=>
��' )
MetodosNativos
��* 8
.
��8 9
CryptSetProvParam
��9 J
(
��J K
providerHandle
��K Y
,
��Y Z
MetodosNativos
�� 
.
�� 
CryptParameter
�� -
.
��- .
KeyExchangePin
��. <
,
��< =
	pinBuffer
�� 
,
�� 
$num
�� 
)
�� 
)
�� 
;
�� 
MetodosNativos
�� 
.
�� 
Executar
�� #
(
��# $
(
��$ %
)
��% &
=>
��' )
MetodosNativos
��* 8
.
��8 9/
!CertSetCertificateContextProperty
��9 Z
(
��Z [
certificado
�� 
.
�� 
Handle
�� "
,
��" #
MetodosNativos
�� 
.
�� !
CertificateProperty
�� 2
.
��2 3"
CryptoProviderHandle
��3 G
,
��G H
$num
�� 
,
�� 
providerHandle
�� !
)
��! "
)
��" #
;
��# $
}
�� 	
private
�� 
static
�� 
X509Certificate2
�� '#
ObterDadosCertificado
��( =
(
��= >%
ConfiguracaoCertificado
��> U%
configuracaoCertificado
��V m
)
��m n
{
�� 	
switch
�� 
(
�� %
configuracaoCertificado
�� +
.
��+ ,
TipoCertificado
��, ;
)
��; <
{
�� 
case
�� 
TipoCertificado
�� $
.
��$ %
A1Repositorio
��% 2
:
��2 3
return
��  
ObterDoRepositorio
�� -
(
��- .%
configuracaoCertificado
��. E
.
��E F
Serial
��F L
,
��L M
	OpenFlags
��N W
.
��W X

MaxAllowed
��X b
)
��b c
;
��c d
case
�� 
TipoCertificado
�� $
.
��$ %
	A1Arquivo
��% .
:
��. /
return
�� 
ObterDeArquivo
�� )
(
��) *%
configuracaoCertificado
��* A
.
��A B
Arquivo
��B I
,
��I J%
configuracaoCertificado
��K b
.
��b c
Senha
��c h
)
��h i
;
��i j
case
�� 
TipoCertificado
�� $
.
��$ %
A3
��% '
:
��' (
return
�� +
ObterDoRepositorioPassandoPin
�� 8
(
��8 9%
configuracaoCertificado
��9 P
.
��P Q
Serial
��Q W
,
��W X%
configuracaoCertificado
��Y p
.
��p q
Senha
��q v
)
��v w
;
��w x
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
��9 :
)
��: ;
;
��; <
}
�� 
}
�� 	
public
�� 
static
�� 
X509Certificate2
�� &'
ListareObterDoRepositorio
��' @
(
��@ A
)
��A B
{
�� 	
var
�� 
store
�� 
=
�� 
ObterX509Store
�� &
(
��& '
	OpenFlags
��' 0
.
��0 1
OpenExistingOnly
��1 A
|
��B C
	OpenFlags
��D M
.
��M N
ReadOnly
��N V
)
��V W
;
��W X
var
�� 

collection
�� 
=
�� 
store
�� "
.
��" #
Certificates
��# /
;
��/ 0
var
�� 
fcollection
�� 
=
�� 

collection
�� (
.
��( )
Find
��) -
(
��- .
X509FindType
��. :
.
��: ;
FindByTimeValid
��; J
,
��J K
DateTime
��L T
.
��T U
Now
��U X
,
��X Y
true
��Z ^
)
��^ _
;
��_ `
var
�� 
scollection
�� 
=
��  
X509Certificate2UI
�� 0
.
��0 1"
SelectFromCollection
��1 E
(
��E F
fcollection
��F Q
,
��Q R
$str
��S j
,
��j k
$str��l �
,��� �
X509SelectionFlag
�� !
.
��! "
SingleSelection
��" 1
)
��1 2
;
��2 3
if
�� 
(
�� 
scollection
�� 
.
�� 
Count
�� !
==
��" $
$num
��% &
)
��& '
{
�� 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ I
)
��I J
;
��J K
}
�� 
store
�� 
.
�� 
Close
�� 
(
�� 
)
�� 
;
�� 
return
�� 
scollection
�� 
[
�� 
$num
��  
]
��  !
;
��! "
}
�� 	
public
�� 
static
�� 
X509Certificate2
�� &
ObterCertificado
��' 7
(
��7 8%
ConfiguracaoCertificado
��8 O%
configuracaoCertificado
��P g
)
��g h
{
�� 	
if
�� 
(
�� 
!
�� %
configuracaoCertificado
�� (
.
��( ) 
ManterDadosEmCache
��) ;
)
��; <
return
�� #
ObterDadosCertificado
�� ,
(
��, -%
configuracaoCertificado
��- D
)
��D E
;
��E F
if
�� 
(
�� 
_certificado
�� 
!=
�� 
null
��  $
)
��$ %
return
�� 
_certificado
�� #
;
��# $
_certificado
�� 
=
�� #
ObterDadosCertificado
�� 0
(
��0 1%
configuracaoCertificado
��1 H
)
��H I
;
��I J
return
�� 
_certificado
�� 
;
��  
}
�� 	
}
�� 
internal
�� 
static
�� 
class
�� 
MetodosNativos
�� (
{
�� 
internal
�� 
enum
�� 
CryptContextFlags
�� '
{
�� 	
None
�� 
=
�� 
$num
�� 
,
�� 
Silent
�� 
=
�� 
$num
�� 
}
�� 	
internal
�� 
enum
�� !
CertificateProperty
�� )
{
�� 	
None
�� 
=
�� 
$num
�� 
,
�� "
CryptoProviderHandle
��  
=
��! "
$num
��# &
}
�� 	
internal
�� 
enum
�� 
CryptParameter
�� $
{
�� 	
None
�� 
=
�� 
$num
�� 
,
�� 
KeyExchangePin
�� 
=
�� 
$num
�� !
}
�� 	
[
�� 	
	DllImport
��	 
(
�� 
$str
�� !
,
��! "
CharSet
��# *
=
��+ ,
CharSet
��- 4
.
��4 5
Auto
��5 9
,
��9 :
SetLastError
��; G
=
��H I
true
��J N
)
��N O
]
��O P
public
�� 
static
�� 
extern
�� 
bool
�� !!
CryptAcquireContext
��" 5
(
��5 6
ref
�� 
IntPtr
�� 
hProv
�� 
,
�� 
string
�� 
containerName
��  
,
��  !
string
�� 
providerName
�� 
,
��  
int
�� 
providerType
�� 
,
�� 
CryptContextFlags
�� 
flags
�� #
)
�� 
;
�� 
[
�� 	
	DllImport
��	 
(
�� 
$str
�� !
,
��! "
SetLastError
��# /
=
��0 1
true
��2 6
,
��6 7
CharSet
��8 ?
=
��@ A
CharSet
��B I
.
��I J
Auto
��J N
)
��N O
]
��O P
public
�� 
static
�� 
extern
�� 
bool
�� !
CryptSetProvParam
��" 3
(
��3 4
IntPtr
�� 
hProv
�� 
,
�� 
CryptParameter
�� 
dwParam
�� "
,
��" #
[
�� 
In
�� 
]
�� 
byte
�� 
[
�� 
]
�� 
pbData
�� 
,
�� 
uint
�� 
dwFlags
�� 
)
�� 
;
�� 
[
�� 	
	DllImport
��	 
(
�� 
$str
��  
,
��  !
SetLastError
��" .
=
��/ 0
true
��1 5
)
��5 6
]
��6 7
internal
�� 
static
�� 
extern
�� 
bool
�� #/
!CertSetCertificateContextProperty
��$ E
(
��E F
IntPtr
�� 
pCertContext
�� 
,
��  !
CertificateProperty
�� 

propertyId
��  *
,
��* +
uint
�� 
dwFlags
�� 
,
�� 
IntPtr
�� 
pvData
�� 
)
�� 
;
�� 
public
�� 
static
�� 
void
�� 
Executar
�� #
(
��# $
Func
��$ (
<
��( )
bool
��) -
>
��- .
action
��/ 5
)
��5 6
{
�� 	
if
�� 
(
�� 
!
�� 
action
�� 
(
�� 
)
�� 
)
�� 
{
�� 
throw
�� 
new
�� 
System
��  
.
��  !
ComponentModel
��! /
.
��/ 0
Win32Exception
��0 >
(
��> ?
Marshal
��? F
.
��F G
GetLastWin32Error
��G X
(
��X Y
)
��Y Z
)
��Z [
;
��[ \
}
�� 
}
�� 	
}
�� 
}�� �A
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\ConfiguracaoCertificado.cs
	namespace&& 	
DFe&&
 
.&& 
Utils&& 
{'' 
public(( 

enum(( 
TipoCertificado(( 
{)) 
[** 	
Description**	 
(** 
$str** %
)**% &
]**& '
A1Repositorio++ 
,++ 
[-- 	
Description--	 
(-- 
$str-- 0
)--0 1
]--1 2
	A1Arquivo.. 
,.. 
[00 	
Description00	 
(00 
$str00 %
)00% &
]00& '
A311 

}22 
public44 

class44 #
ConfiguracaoCertificado44 (
:44) *"
INotifyPropertyChanged44+ A
{55 
private66 
string66 
_serial66 
;66 
private77 
string77 
_arquivo77 
;77  
private88 
string88 
_senha88 
;88 
private99 
TipoCertificado99 
_tipoCertificado99  0
;990 1
public>> 
TipoCertificado>> 
TipoCertificado>> .
{?? 	
get@@ 
{@@ 
return@@ 
_tipoCertificado@@ )
;@@) *
}@@+ ,
setAA 
{BB 
SerialCC 
=CC 
nullCC 
;CC 
ArquivoDD 
=DD 
nullDD 
;DD 
SenhaEE 
=EE 
nullEE 
;EE 
_tipoCertificadoFF  
=FF! "
valueFF# (
;FF( )
OnPropertyChangedGG !
(GG! "
thisGG" &
.GG& ' 
ObterPropriedadeInfoGG' ;
(GG; <
cGG< =
=>GG> @
cGGA B
.GGB C
TipoCertificadoGGC R
)GGR S
.GGS T
NameGGT X
)GGX Y
;GGY Z
}HH 
}II 	
publicNN 
stringNN 
SerialNN 
{OO 	
getPP 
{PP 
returnPP 
_serialPP  
;PP  !
}PP" #
setQQ 
{RR 
ifSS 
(SS 
!SS 
stringSS 
.SS 
IsNullOrEmptySS )
(SS) *
valueSS* /
)SS/ 0
&&SS1 3
TipoCertificadoSS4 C
==SSD F
TipoCertificadoSSG V
.SSV W
	A1ArquivoSSW `
)SS` a
throwTT 
newTT 
	ExceptionTT '
(TT' (
stringTT( .
.TT. /
FormatTT/ 5
(TT5 6
$strTT6 ^
,TT^ _
TipoCertificadoTT` o
.TTo p
	DescricaoTTp y
(TTy z
)TTz {
,TT{ |
this	TT} �
.
TT� �"
ObterPropriedadeInfo
TT� �
(
TT� �
c
TT� �
=>
TT� �
c
TT� �
.
TT� �
Serial
TT� �
)
TT� �
.
TT� �
Name
TT� �
)
TT� �
)
TT� �
;
TT� �
ifVV 
(VV 
valueVV 
==VV 
_serialVV $
)VV$ %
returnVV& ,
;VV, -
_serialWW 
=WW 
valueWW 
;WW  
ifXX 
(XX 
!XX 
stringXX 
.XX 
IsNullOrEmptyXX )
(XX) *
valueXX* /
)XX/ 0
)XX0 1
ArquivoYY 
=YY 
nullYY "
;YY" #
OnPropertyChangedZZ !
(ZZ! "
thisZZ" &
.ZZ& ' 
ObterPropriedadeInfoZZ' ;
(ZZ; <
cZZ< =
=>ZZ> @
cZZA B
.ZZB C
SerialZZC I
)ZZI J
.ZZJ K
NameZZK O
)ZZO P
;ZZP Q
}[[ 
}\\ 	
publicaa 
stringaa 
Arquivoaa 
{bb 	
getcc 
{cc 
returncc 
_arquivocc !
;cc! "
}cc# $
setdd 
{ee 
ifff 
(ff 
!ff 
stringff 
.ff 
IsNullOrEmptyff )
(ff) *
valueff* /
)ff/ 0
&&ff1 3
TipoCertificadoff4 C
!=ffD F
TipoCertificadoffG V
.ffV W
	A1ArquivoffW `
)ff` a
throwgg 
newgg 
	Exceptiongg '
(gg' (
stringgg( .
.gg. /
Formatgg/ 5
(gg5 6
$strgg6 ^
,gg^ _
TipoCertificadogg` o
.ggo p
	Descricaoggp y
(ggy z
)ggz {
,gg{ |
this	gg} �
.
gg� �"
ObterPropriedadeInfo
gg� �
(
gg� �
c
gg� �
=>
gg� �
c
gg� �
.
gg� �
Arquivo
gg� �
)
gg� �
.
gg� �
Name
gg� �
)
gg� �
)
gg� �
;
gg� �
ifhh 
(hh 
valuehh 
==hh 
_arquivohh %
)hh% &
returnhh' -
;hh- .
_arquivoii 
=ii 
valueii  
;ii  !
ifjj 
(jj 
!jj 
stringjj 
.jj 
IsNullOrEmptyjj )
(jj) *
valuejj* /
)jj/ 0
)jj0 1
Serialkk 
=kk 
nullkk !
;kk! "
OnPropertyChangedll !
(ll! "
thisll" &
.ll& ' 
ObterPropriedadeInfoll' ;
(ll; <
cll< =
=>ll> @
cllA B
.llB C
ArquivollC J
)llJ K
.llK L
NamellL P
)llP Q
;llQ R
}mm 
}nn 	
publicuu 
stringuu 
Senhauu 
{vv 	
getww 
{ww 
returnww 
_senhaww 
;ww  
}ww! "
setxx 
{yy 
ifzz 
(zz 
!zz 
stringzz 
.zz 
IsNullOrEmptyzz )
(zz) *
valuezz* /
)zz/ 0
&&zz1 3
TipoCertificadozz4 C
==zzD F
TipoCertificadozzG V
.zzV W
A1RepositoriozzW d
)zzd e
throw{{ 
new{{ 
	Exception{{ '
({{' (
string{{( .
.{{. /
Format{{/ 5
({{5 6
$str{{6 ^
,{{^ _
TipoCertificado{{` o
.{{o p
	Descricao{{p y
({{y z
){{z {
,{{{ |
this	{{} �
.
{{� �"
ObterPropriedadeInfo
{{� �
(
{{� �
c
{{� �
=>
{{� �
c
{{� �
.
{{� �
Senha
{{� �
)
{{� �
.
{{� �
Name
{{� �
)
{{� �
)
{{� �
;
{{� �
if|| 
(|| 
value|| 
==|| 
_senha|| #
)||# $
return||% +
;||+ ,
_senha}} 
=}} 
value}} 
;}} 
OnPropertyChanged~~ !
(~~! "
this~~" &
.~~& ' 
ObterPropriedadeInfo~~' ;
(~~; <
c~~< =
=>~~> @
c~~A B
.~~B C
Senha~~C H
)~~H I
.~~I J
Name~~J N
)~~N O
;~~O P
} 
}
�� 	
public
�� 
bool
��  
ManterDadosEmCache
�� &
{
��' (
get
��) ,
;
��, -
set
��. 1
;
��1 2
}
��3 4
public
�� 
event
�� )
PropertyChangedEventHandler
�� 0
PropertyChanged
��1 @
;
��@ A
[
�� 	,
NotifyPropertyChangedInvocator
��	 '
]
��' (
	protected
�� 
virtual
�� 
void
�� 
OnPropertyChanged
�� 0
(
��0 1
string
��1 7
propertyName
��8 D
)
��D E
{
�� 	
if
�� 
(
�� 
PropertyChanged
�� 
!=
��  "
null
��# '
)
��' (
PropertyChanged
��) 8
.
��8 9
Invoke
��9 ?
(
��? @
this
��@ D
,
��D E
new
��F I&
PropertyChangedEventArgs
��J b
(
��b c
propertyName
��c o
)
��o p
)
��p q
;
��q r
}
�� 	
}
�� 
}�� �
DC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\DataHora.cs
	namespace## 	
DFe##
 
.## 
Utils## 
{$$ 
public%% 

static%% 
class%% 
DataHora%%  
{&& 
public,, 
static,, 
string,, 
ParaDataString,, +
(,,+ ,
this,,, 0
DateTime,,1 9
data,,: >
),,> ?
{-- 	
return.. 
data.. 
==.. 
DateTime.. #
...# $
MinValue..$ ,
?..- .
null../ 3
:..4 5
data..6 :
...: ;
ToString..; C
(..C D
$str..D P
)..P Q
;..Q R
}// 	
public66 
static66 
string66 !
ParaDataHoraStringUtc66 2
(662 3
this663 7
DateTime668 @
data66A E
)66E F
{77 	
return88 
data88 
==88 
DateTime88 #
.88# $
MinValue88$ ,
?88- .
null88/ 3
:884 5
data886 :
.88: ;
ToString88; C
(88C D
$str88D \
)88\ ]
;88] ^
}99 	
public@@ 
static@@ 
string@@ $
ParaDataHoraStringSemUtc@@ 5
(@@5 6
this@@6 :
DateTime@@; C
data@@D H
)@@H I
{AA 	
returnBB 
dataBB 
.BB 
ToStringBB  
(BB  !
$strBB! 6
)BB6 7
;BB7 8
}CC 	
publicJJ 
staticJJ 
stringJJ !
ParaDataHoraStringUtcJJ 2
(JJ2 3
thisJJ3 7
DateTimeJJ8 @
?JJ@ A
dataJJB F
)JJF G
{KK 	
returnLL !
ParaDataHoraStringUtcLL (
(LL( )
dataLL) -
.LL- .
GetValueOrDefaultLL. ?
(LL? @
)LL@ A
)LLA B
;LLB C
}MM 	
publicTT 
staticTT 
stringTT 
ParaDataHoraStringTT /
(TT/ 0
thisTT0 4
DateTimeTT5 =
dataTT> B
)TTB C
{UU 	
returnVV 
dataVV 
.VV 
ToStringVV  
(VV  !
$strVV! 1
)VV1 2
;VV2 3
}WW 	
}XX 
}YY �
CC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\EnumExt.cs
	namespace 	
DFe
 
. 
Utils 
{ 
public 

static 
class 
EnumExt 
{ 
public 
static 
T 
ObterAtributo %
<% &
T& '
>' (
(( )
this) -
Enum. 2
value3 8
)8 9
where: ?
T@ A
:B C
	AttributeD M
{ 	
var 
type 
= 
value 
. 
GetType $
($ %
)% &
;& '
var 

memberInfo 
= 
type !
.! "
	GetMember" +
(+ ,
value, 1
.1 2
ToString2 :
(: ;
); <
)< =
;= >
var 

attributes 
= 

memberInfo '
[' (
$num( )
]) *
.* +
GetCustomAttributes+ >
(> ?
typeof? E
(E F
TF G
)G H
,H I
falseJ O
)O P
;P Q
return 
( 
T 
) 

attributes  
[  !
$num! "
]" #
;# $
} 	
public 
static 
string 
	Descricao &
(& '
this' +
Enum, 0
value1 6
)6 7
{   	
var!! 
	attribute!! 
=!! 
value!! !
.!!! "
ObterAtributo!!" /
<!!/ 0 
DescriptionAttribute!!0 D
>!!D E
(!!E F
)!!F G
;!!G H
return"" 
	attribute"" 
=="" 
null""  $
?""% &
value""' ,
."", -
ToString""- 5
(""5 6
)""6 7
:""8 9
	attribute"": C
.""C D
Description""D O
;""O P
}## 	
}%% 
}&& �^
FC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\FuncoesXml.cs
	namespace)) 	
DFe))
 
.)) 
Utils)) 
{** 
public++ 

static++ 
class++ 

FuncoesXml++ "
{,, 
public33 
static33 
string33 
ClasseParaXmlString33 0
<330 1
T331 2
>332 3
(333 4
T334 5
objeto336 <
)33< =
{44 	
XElement55 
xml55 
;55 
var66 
ser66 
=66 
new66 
XmlSerializer66 '
(66' (
typeof66( .
(66. /
T66/ 0
)660 1
)661 2
;662 3
using88 
(88 
var88 
memory88 
=88 
new88  #
MemoryStream88$ 0
(880 1
)881 2
)882 3
{99 
using:: 
(:: 

TextReader:: !
tr::" $
=::% &
new::' *
StreamReader::+ 7
(::7 8
memory::8 >
,::> ?
Encoding::@ H
.::H I
UTF8::I M
)::M N
)::N O
{;; 
ser<< 
.<< 
	Serialize<< !
(<<! "
memory<<" (
,<<( )
objeto<<* 0
)<<0 1
;<<1 2
memory== 
.== 
Position== #
===$ %
$num==& '
;==' (
xml>> 
=>> 
XElement>> "
.>>" #
Load>># '
(>>' (
tr>>( *
)>>* +
;>>+ ,
xml?? 
.?? 

Attributes?? "
(??" #
)??# $
.??$ %
Where??% *
(??* +
x??+ ,
=>??- /
x??0 1
.??1 2
Name??2 6
.??6 7
	LocalName??7 @
.??@ A
Equals??A G
(??G H
$str??H M
)??M N
||??O Q
x??R S
.??S T
Name??T X
.??X Y
	LocalName??Y b
.??b c
Equals??c i
(??i j
$str??j o
)??o p
)??p q
.??q r
Remove??r x
(??x y
)??y z
;??z {
}@@ 
}AA 
returnBB 
XElementBB 
.BB 
ParseBB !
(BB! "
xmlBB" %
.BB% &
ToStringBB& .
(BB. /
)BB/ 0
)BB0 1
.BB1 2
ToStringBB2 :
(BB: ;
SaveOptionsBB; F
.BBF G
DisableFormattingBBG X
)BBX Y
;BBY Z
}CC 	
publicKK 
staticKK 
TKK 
XmlStringParaClasseKK +
<KK+ ,
TKK, -
>KK- .
(KK. /
stringKK/ 5
inputKK6 ;
)KK; <
whereKK= B
TKKC D
:KKE F
classKKG L
{LL 	
varMM 
serMM 
=MM 
newMM 
XmlSerializerMM '
(MM' (
typeofMM( .
(MM. /
TMM/ 0
)MM0 1
)MM1 2
;MM2 3
usingOO 
(OO 
varOO 
srOO 
=OO 
newOO 
StringReaderOO  ,
(OO, -
inputOO- 2
)OO2 3
)OO3 4
returnPP 
(PP 
TPP 
)PP 
serPP 
.PP 
DeserializePP )
(PP) *
srPP* ,
)PP, -
;PP- .
}QQ 	
publicZZ 
staticZZ 
TZZ  
ArquivoXmlParaClasseZZ ,
<ZZ, -
TZZ- .
>ZZ. /
(ZZ/ 0
stringZZ0 6
arquivoZZ7 >
)ZZ> ?
whereZZ@ E
TZZF G
:ZZH I
classZZJ O
{[[ 	
if\\ 
(\\ 
!\\ 
File\\ 
.\\ 
Exists\\ 
(\\ 
arquivo\\ $
)\\$ %
)\\% &
{]] 
throw^^ 
new^^ !
FileNotFoundException^^ /
(^^/ 0
$str^^0 :
+^^; <
arquivo^^= D
+^^E F
$str^^G Y
)^^Y Z
;^^Z [
}__ 
varaa 
serializadoraa 
=aa 
newaa "
XmlSerializeraa# 0
(aa0 1
typeofaa1 7
(aa7 8
Taa8 9
)aa9 :
)aa: ;
;aa; <
varbb 
streambb 
=bb 
newbb 

FileStreambb '
(bb' (
arquivobb( /
,bb/ 0
FileModebb1 9
.bb9 :
Openbb: >
,bb> ?

FileAccessbb@ J
.bbJ K
ReadbbK O
,bbO P
	FileSharebbQ Z
.bbZ [
	ReadWritebb[ d
)bbd e
;bbe f
trycc 
{dd 
returnee 
(ee 
Tee 
)ee 
serializadoree &
.ee& '
Deserializeee' 2
(ee2 3
streamee3 9
)ee9 :
;ee: ;
}ff 
finallygg 
{hh 
streamii 
.ii 
Closeii 
(ii 
)ii 
;ii 
}jj 
}kk 	
publictt 
statictt 
voidtt  
ClasseParaArquivoXmltt /
<tt/ 0
Ttt0 1
>tt1 2
(tt2 3
Ttt3 4
objetott5 ;
,tt; <
stringtt= C
arquivottD K
)ttK L
{uu 	
varvv 
dirvv 
=vv 
Pathvv 
.vv 
GetDirectoryNamevv +
(vv+ ,
arquivovv, 3
)vv3 4
;vv4 5
ifww 
(ww 
dirww 
!=ww 
nullww 
&&ww 
!ww  
	Directoryww  )
.ww) *
Existsww* 0
(ww0 1
dirww1 4
)ww4 5
)ww5 6
{xx 
throwyy 
newyy &
DirectoryNotFoundExceptionyy 4
(yy4 5
$stryy5 A
+yyB C
diryyD G
+yyH I
$stryyJ \
)yy\ ]
;yy] ^
}zz 
var|| 
xml|| 
=|| 
ClasseParaXmlString|| )
(||) *
objeto||* 0
)||0 1
;||1 2
try}} 
{~~ 
var 
stw 
= 
new 
StreamWriter *
(* +
arquivo+ 2
)2 3
;3 4
stw
�� 
.
�� 
	WriteLine
�� 
(
�� 
xml
�� !
)
��! "
;
��" #
stw
�� 
.
�� 
Close
�� 
(
�� 
)
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
)
�� 
{
�� 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ G
+
��H I
arquivo
��J Q
+
��R S
$str
��T W
)
��W X
;
��X Y
}
�� 
}
�� 	
public
�� 
static
�� 
void
�� +
SalvarStringXmlParaArquivoXml
�� 8
(
��8 9
string
��9 ?
xml
��@ C
,
��C D
string
��E K
arquivo
��L S
)
��S T
{
�� 	
var
�� 
dir
�� 
=
�� 
Path
�� 
.
�� 
GetDirectoryName
�� +
(
��+ ,
arquivo
��, 3
)
��3 4
;
��4 5
if
�� 
(
�� 
dir
�� 
!=
�� 
null
�� 
&&
�� 
!
��  
	Directory
��  )
.
��) *
Exists
��* 0
(
��0 1
dir
��1 4
)
��4 5
)
��5 6
{
�� 
throw
�� 
new
�� (
DirectoryNotFoundException
�� 4
(
��4 5
$str
��5 A
+
��B C
dir
��D G
+
��H I
$str
��J \
)
��\ ]
;
��] ^
}
�� 
try
�� 
{
�� 
var
�� 
stw
�� 
=
�� 
new
�� 
StreamWriter
�� *
(
��* +
arquivo
��+ 2
)
��2 3
;
��3 4
stw
�� 
.
�� 
	WriteLine
�� 
(
�� 
xml
�� !
)
��! "
;
��" #
stw
�� 
.
�� 
Close
�� 
(
�� 
)
�� 
;
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
)
�� 
{
�� 
throw
�� 
new
�� 
	Exception
�� #
(
��# $
$str
��$ G
+
��H I
arquivo
��J Q
+
��R S
$str
��T W
)
��W X
;
��X Y
}
�� 
}
�� 	
public
�� 
static
�� 
string
�� #
ObterNodeDeArquivoXml
�� 2
(
��2 3
string
��3 9

nomeDoNode
��: D
,
��D E
string
��F L

arquivoXml
��M W
)
��W X
{
�� 	
var
�� 
xmlDoc
�� 
=
�� 
	XDocument
�� "
.
��" #
Load
��# '
(
��' (

arquivoXml
��( 2
)
��2 3
;
��3 4
var
�� 
	xmlString
�� 
=
�� 
(
�� 
from
�� !
d
��" #
in
��$ &
xmlDoc
��' -
.
��- .
Descendants
��. 9
(
��9 :
)
��: ;
where
�� "
d
��# $
.
��$ %
Name
��% )
.
��) *
	LocalName
��* 3
==
��4 6

nomeDoNode
��7 A
select
�� #
d
��$ %
)
��% &
.
��& '
FirstOrDefault
��' 5
(
��5 6
)
��6 7
;
��7 8
if
�� 
(
�� 
	xmlString
�� 
==
�� 
null
�� !
)
��! "
throw
�� 
new
�� 
	Exception
�� #
(
��# $
String
��$ *
.
��* +
Format
��+ 1
(
��1 2
$str
��2 `
,
��` a

nomeDoNode
��b l
,
��l m

arquivoXml
��n x
)
��x y
)
��y z
;
��z {
return
�� 
	xmlString
�� 
.
�� 
ToString
�� %
(
��% &
)
��& '
;
��' (
}
�� 	
public
�� 
static
�� 
string
�� "
ObterNodeDeStringXml
�� 1
(
��1 2
string
��2 8

nomeDoNode
��9 C
,
��C D
string
��E K
	stringXml
��L U
)
��U V
{
�� 	
var
�� 
s
�� 
=
�� 
	stringXml
�� 
;
�� 
var
�� 
xmlDoc
�� 
=
�� 
	XDocument
�� "
.
��" #
Parse
��# (
(
��( )
s
��) *
)
��* +
;
��+ ,
var
�� 
	xmlString
�� 
=
�� 
(
�� 
from
�� !
d
��" #
in
��$ &
xmlDoc
��' -
.
��- .
Descendants
��. 9
(
��9 :
)
��: ;
where
�� "
d
��# $
.
��$ %
Name
��% )
.
��) *
	LocalName
��* 3
==
��4 6

nomeDoNode
��7 A
select
�� #
d
��$ %
)
��% &
.
��& '
FirstOrDefault
��' 5
(
��5 6
)
��6 7
;
��7 8
if
�� 
(
�� 
	xmlString
�� 
==
�� 
null
�� !
)
��! "
throw
�� 
new
�� 
	Exception
�� #
(
��# $
String
��$ *
.
��* +
Format
��+ 1
(
��1 2
$str
��2 X
,
��X Y

nomeDoNode
��Z d
)
��d e
)
��e f
;
��f g
return
�� 
	xmlString
�� 
.
�� 
ToString
�� %
(
��% &
)
��& '
;
��' (
}
�� 	
}
�� 
}�� �S
GC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\ChaveFiscal.cs
	namespace&& 	
DFe&&
 
.&& 
Utils&& 
{'' 
public++ 

class++ 
ChaveFiscal++ 
{,, 
public99 
static99 
DadosChaveFiscal99 &

ObterChave99' 1
(991 2
	EstadoNfe992 ;

ufEmitente99< F
,99F G
DateTime99H P
dataEmissao99Q \
,99\ ]
string99^ d
cnpjEmitente99e q
,99q r
ModeloDocumento	99s �
modelo
99� �
,
99� �
int
99� �
serie
99� �
,
99� �
long
99� �
numero
99� �
,
99� �
int
99� �
tipoEmissao
99� �
,
99� �
int
99� �
cNf
99� �
)
99� �
{:: 	
var;; 
chave;; 
=;; 
new;; 
StringBuilder;; )
(;;) *
);;* +
;;;+ ,
chave== 
.== 
Append== 
(== 
(== 
(== 
int== 
)== 

ufEmitente== )
)==) *
.==* +
ToString==+ 3
(==3 4
$str==4 8
)==8 9
)==9 :
.>> 
Append>> 
(>> 
Convert>> 
.>>  

ToDateTime>>  *
(>>* +
dataEmissao>>+ 6
)>>6 7
.>>7 8
ToString>>8 @
(>>@ A
$str>>A G
)>>G H
)>>H I
.?? 
Append?? 
(?? 
cnpjEmitente?? $
)??$ %
.@@ 
Append@@ 
(@@ 
(@@ 
(@@ 
int@@ 
)@@ 
modelo@@ $
)@@$ %
.@@% &
ToString@@& .
(@@. /
$str@@/ 3
)@@3 4
)@@4 5
.AA 
AppendAA 
(AA 
serieAA 
.AA 
ToStringAA &
(AA& '
$strAA' +
)AA+ ,
)AA, -
.BB 
AppendBB 
(BB 
numeroBB 
.BB 
ToStringBB '
(BB' (
$strBB( ,
)BB, -
)BB- .
.CC 
AppendCC 
(CC 
tipoEmissaoCC #
.CC# $
ToStringCC$ ,
(CC, -
)CC- .
)CC. /
.DD 
AppendDD 
(DD 
cNfDD 
.DD 
ToStringDD $
(DD$ %
$strDD% )
)DD) *
)DD* +
;DD+ ,
varFF 
digitoVerificadorFF !
=FF" #"
ObterDigitoVerificadorFF$ :
(FF: ;
chaveFF; @
.FF@ A
ToStringFFA I
(FFI J
)FFJ K
)FFK L
;FFL M
chaveHH 
.HH 
AppendHH 
(HH 
digitoVerificadorHH *
)HH* +
;HH+ ,
returnJJ 
newJJ 
DadosChaveFiscalJJ '
(JJ' (
chaveJJ( -
.JJ- .
ToStringJJ. 6
(JJ6 7
)JJ7 8
,JJ8 9
byteJJ: >
.JJ> ?
ParseJJ? D
(JJD E
digitoVerificadorJJE V
)JJV W
)JJW X
;JJX Y
}KK 	
privateRR 
staticRR 
stringRR "
ObterDigitoVerificadorRR 4
(RR4 5
stringRR5 ;
chaveRR< A
)RRA B
{SS 	
varTT 
somaTT 
=TT 
$numTT 
;TT 
varUU 
modUU 
=UU 
-UU 
$numUU 
;UU 
varVV 
dvVV 
=VV 
-VV 
$numVV 
;VV 
varWW 
pesoWW 
=WW 
$numWW 
;WW 
forZZ 
(ZZ 
varZZ 
iZZ 
=ZZ 
chaveZZ 
.ZZ 
LengthZZ %
-ZZ& '
$numZZ( )
;ZZ) *
iZZ+ ,
!=ZZ- /
-ZZ0 1
$numZZ1 2
;ZZ2 3
iZZ4 5
--ZZ5 7
)ZZ7 8
{[[ 
var\\ 
ch\\ 
=\\ 
Convert\\  
.\\  !
ToInt32\\! (
(\\( )
chave\\) .
[\\. /
i\\/ 0
]\\0 1
.\\1 2
ToString\\2 :
(\\: ;
)\\; <
)\\< =
;\\= >
soma]] 
+=]] 
ch]] 
*]] 
peso]] !
;]]! "
if__ 
(__ 
peso__ 
<__ 
$num__ 
)__ 
peso`` 
+=`` 
$num`` 
;`` 
elseaa 
pesobb 
=bb 
$numbb 
;bb 
}cc 
modff 
=ff 
somaff 
%ff 
$numff 
;ff 
ifhh 
(hh 
modhh 
==hh 
$numhh 
||hh 
modhh 
==hh  "
$numhh# $
)hh$ %
dvii 
=ii 
$numii 
;ii 
elsejj 
dvkk 
=kk 
$numkk 
-kk 
modkk 
;kk 
returnmm 
dvmm 
.mm 
ToStringmm 
(mm 
)mm  
;mm  !
}nn 	
publicuu 
staticuu 
booluu 
ChaveValidauu &
(uu& '
stringuu' -
chaveNfeuu. 6
)uu6 7
{vv 	
	EstadoNfeww 
codigoww 
;ww 
Enumxx 
.xx 
TryParsexx 
(xx 
chaveNfexx "
.xx" #
	Substringxx# ,
(xx, -
$numxx- .
,xx. /
$numxx0 1
)xx1 2
,xx2 3
outxx4 7
codigoxx8 >
)xx> ?
;xx? @
varzz 
anoMeszz 
=zz 
chaveNfezz !
.zz! "
	Substringzz" +
(zz+ ,
$numzz, -
,zz- .
$numzz/ 0
)zz0 1
;zz1 2
var|| 
ano|| 
=|| 
int|| 
.|| 
Parse|| 
(||  
anoMes||  &
.||& '
	Substring||' 0
(||0 1
$num||1 2
,||2 3
$num||4 5
)||5 6
)||6 7
;||7 8
var}} 
mes}} 
=}} 
int}} 
.}} 
Parse}} 
(}}  
anoMes}}  &
.}}& '
	Substring}}' 0
(}}0 1
$num}}1 2
,}}2 3
$num}}4 5
)}}5 6
)}}6 7
;}}7 8
var~~ 
anoEMesData~~ 
=~~ 
new~~ !
DateTime~~" *
(~~* +
ano~~+ .
,~~. /
mes~~0 3
,~~3 4
$num~~5 6
)~~6 7
;~~7 8
var
�� 
cnpj
�� 
=
�� 
chaveNfe
�� 
.
��  
	Substring
��  )
(
��) *
$num
��* +
,
��+ ,
$num
��- /
)
��/ 0
;
��0 1
ModeloDocumento
�� 
modelo
�� "
;
��" #
Enum
�� 
.
�� 
TryParse
�� 
(
�� 
chaveNfe
�� "
.
��" #
	Substring
��# ,
(
��, -
$num
��- /
,
��/ 0
$num
��1 2
)
��2 3
,
��3 4
out
��5 8
modelo
��9 ?
)
��? @
;
��@ A
var
�� 
serie
�� 
=
�� 
int
�� 
.
�� 
Parse
�� !
(
��! "
chaveNfe
��" *
.
��* +
	Substring
��+ 4
(
��4 5
$num
��5 7
,
��7 8
$num
��9 :
)
��: ;
)
��; <
;
��< =
var
�� 
	numeroNfe
�� 
=
�� 
long
��  
.
��  !
Parse
��! &
(
��& '
chaveNfe
��' /
.
��/ 0
	Substring
��0 9
(
��9 :
$num
��: <
,
��< =
$num
��> ?
)
��? @
)
��@ A
;
��A B
var
�� 
formaEmissao
�� 
=
�� 
int
�� "
.
��" #
Parse
��# (
(
��( )
chaveNfe
��) 1
.
��1 2
	Substring
��2 ;
(
��; <
$num
��< >
,
��> ?
$num
��@ A
)
��A B
)
��B C
;
��C D
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
chaveNfe
��+ 3
.
��3 4
	Substring
��4 =
(
��= >
$num
��> @
,
��@ A
$num
��B C
)
��C D
)
��D E
;
��E F
var
�� 
digitoVerificador
�� !
=
��" #
chaveNfe
��$ ,
.
��, -
	Substring
��- 6
(
��6 7
$num
��7 9
,
��9 :
$num
��; <
)
��< =
;
��= >
var
�� 

gerarChave
�� 
=
�� 

ObterChave
�� '
(
��' (
codigo
��( .
,
��. /
anoEMesData
��0 ;
,
��; <
cnpj
��= A
,
��A B
modelo
��C I
,
��I J
serie
��K P
,
��P Q
	numeroNfe
��R [
,
��[ \
formaEmissao
��] i
,
��i j
codigoNumerico
��k y
)
��y z
;
��z {
return
�� 
digitoVerificador
�� $
.
��$ %
Equals
��% +
(
��+ ,

gerarChave
��, 6
.
��6 7
DigitoVerificador
��7 H
.
��H I
ToString
��I Q
(
��Q R
)
��R S
)
��S T
;
��T U
}
�� 	
}
�� 
public
�� 

class
�� 
DadosChaveFiscal
�� !
{
�� 
public
�� 
DadosChaveFiscal
�� 
(
��  
string
��  &
chave
��' ,
,
��, -
byte
��. 2
digitoVerificador
��3 D
)
��D E
{
�� 	
Chave
�� 
=
�� 
chave
�� 
;
�� 
DigitoVerificador
�� 
=
�� 
digitoVerificador
��  1
;
��1 2
}
�� 	
public
�� 
string
�� 
Chave
�� 
{
�� 
get
�� !
;
��! "
private
��# *
set
��+ .
;
��. /
}
��0 1
public
�� 
byte
�� 
DigitoVerificador
�� %
{
��& '
get
��( +
;
��+ ,
private
��- 4
set
��5 8
;
��8 9
}
��: ;
}
�� 
}�� ۃ
RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\Properties\Annotations.cs
	namespace 	
NFe
 
. 
Utils 
. 
Annotations 
{ 
[ 
AttributeUsage 
( 
AttributeTargets 
. 
Method 
| 
AttributeTargets .
.. /
	Parameter/ 8
|9 :
AttributeTargets; K
.K L
PropertyL T
|U V
AttributeTargets 
. 
Delegate 
| 
AttributeTargets  0
.0 1
Field1 6
|7 8
AttributeTargets9 I
.I J
EventJ O
)O P
]P Q
public 

sealed 
class 
CanBeNullAttribute *
:+ ,
	Attribute- 6
{7 8
}9 :
[&& 
AttributeUsage&& 
(&& 
AttributeTargets'' 
.'' 
Method'' 
|'' 
AttributeTargets''  0
.''0 1
	Parameter''1 :
|''; <
AttributeTargets''= M
.''M N
Property''N V
|''W X
AttributeTargets(( 
.(( 
Delegate(( 
|((  !
AttributeTargets((" 2
.((2 3
Field((3 8
|((9 :
AttributeTargets((; K
.((K L
Event((L Q
)((Q R
]((R S
public)) 

sealed)) 
class)) 
NotNullAttribute)) (
:))) *
	Attribute))+ 4
{))5 6
}))7 8
[00 
AttributeUsage00 
(00 
AttributeTargets11 
.11 
Method11 
|11 
AttributeTargets11  0
.110 1
	Parameter111 :
|11; <
AttributeTargets11= M
.11M N
Property11N V
|11W X
AttributeTargets22 
.22 
Delegate22 
|22  !
AttributeTargets22" 2
.222 3
Field223 8
)228 9
]229 :
public33 

sealed33 
class33  
ItemNotNullAttribute33 ,
:33- .
	Attribute33/ 8
{339 :
}33; <
[:: 
AttributeUsage:: 
(:: 
AttributeTargets;; 
.;; 
Method;; 
|;; 
AttributeTargets;;  0
.;;0 1
	Parameter;;1 :
|;;; <
AttributeTargets;;= M
.;;M N
Property;;N V
|;;W X
AttributeTargets<< 
.<< 
Delegate<< 
|<<  !
AttributeTargets<<" 2
.<<2 3
Field<<3 8
)<<8 9
]<<9 :
public== 

sealed== 
class== "
ItemCanBeNullAttribute== .
:==/ 0
	Attribute==1 :
{==; <
}=== >
[KK 
AttributeUsageKK 
(KK 
AttributeTargetsLL 
.LL 
ConstructorLL "
|LL# $
AttributeTargetsLL% 5
.LL5 6
MethodLL6 <
|LL= >
AttributeTargetsMM 
.MM 
PropertyMM 
|MM  !
AttributeTargetsMM" 2
.MM2 3
DelegateMM3 ;
)MM; <
]MM< =
publicNN 

sealedNN 
classNN '
StringFormatMethodAttributeNN 3
:NN4 5
	AttributeNN6 ?
{OO 
publicSS '
StringFormatMethodAttributeSS *
(SS* +
stringSS+ 1
formatParameterNameSS2 E
)SSE F
{TT 	
FormatParameterNameUU 
=UU  !
formatParameterNameUU" 5
;UU5 6
}VV 	
publicXX 
stringXX 
FormatParameterNameXX )
{XX* +
getXX, /
;XX/ 0
privateXX1 8
setXX9 <
;XX< =
}XX> ?
}YY 
[__ 
AttributeUsage__ 
(__ 
AttributeTargets__ $
.__$ %
	Parameter__% .
|__/ 0
AttributeTargets__1 A
.__A B
Property__B J
|__K L
AttributeTargets__M ]
.__] ^
Field__^ c
)__c d
]__d e
public`` 

sealed`` 
class`` "
ValueProviderAttribute`` .
:``/ 0
	Attribute``1 :
{aa 
publicbb "
ValueProviderAttributebb %
(bb% &
stringbb& ,
namebb- 1
)bb1 2
{cc 	
Namedd 
=dd 
namedd 
;dd 
}ee 	
[gg 	
NotNullgg	 
]gg 
publicgg 
stringgg 
Namegg  $
{gg% &
getgg' *
;gg* +
privategg, 3
setgg4 7
;gg7 8
}gg9 :
}hh 
[uu 
AttributeUsageuu 
(uu 
AttributeTargetsuu $
.uu$ %
	Parameteruu% .
)uu. /
]uu/ 0
publicvv 

sealedvv 
classvv )
InvokerParameterNameAttributevv 5
:vv6 7
	Attributevv8 A
{vvB C
}vvD E
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� 5
'NotifyPropertyChangedInvocatorAttribute
�� ?
:
��@ A
	Attribute
��B K
{
�� 
public
�� 5
'NotifyPropertyChangedInvocatorAttribute
�� 6
(
��6 7
)
��7 8
{
��9 :
}
��; <
public
�� 5
'NotifyPropertyChangedInvocatorAttribute
�� 6
(
��6 7
string
��7 =
parameterName
��> K
)
��K L
{
�� 	
ParameterName
�� 
=
�� 
parameterName
�� )
;
��) *
}
�� 	
public
�� 
string
�� 
ParameterName
�� #
{
��$ %
get
��& )
;
��) *
private
��+ 2
set
��3 6
;
��6 7
}
��8 9
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
,
��+ ,
AllowMultiple
��- :
=
��; <
true
��= A
)
��A B
]
��B C
public
�� 

sealed
�� 
class
�� )
ContractAnnotationAttribute
�� 3
:
��4 5
	Attribute
��6 ?
{
�� 
public
�� )
ContractAnnotationAttribute
�� *
(
��* +
[
��+ ,
NotNull
��, 3
]
��3 4
string
��5 ;
contract
��< D
)
��D E
:
��
 
this
�� 
(
�� 
contract
�� 
,
�� 
false
��  
)
��  !
{
��" #
}
��$ %
public
�� )
ContractAnnotationAttribute
�� *
(
��* +
[
��+ ,
NotNull
��, 3
]
��3 4
string
��5 ;
contract
��< D
,
��D E
bool
��F J
forceFullStates
��K Z
)
��Z [
{
�� 	
Contract
�� 
=
�� 
contract
�� 
;
��  
ForceFullStates
�� 
=
�� 
forceFullStates
�� -
;
��- .
}
�� 	
public
�� 
string
�� 
Contract
�� 
{
��  
get
��! $
;
��$ %
private
��& -
set
��. 1
;
��1 2
}
��3 4
public
�� 
bool
�� 
ForceFullStates
�� #
{
��$ %
get
��& )
;
��) *
private
��+ 2
set
��3 6
;
��6 7
}
��8 9
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
All
��% (
)
��( )
]
��) *
public
�� 

sealed
�� 
class
�� +
LocalizationRequiredAttribute
�� 5
:
��6 7
	Attribute
��8 A
{
�� 
public
�� +
LocalizationRequiredAttribute
�� ,
(
��, -
)
��- .
:
��/ 0
this
��1 5
(
��5 6
true
��6 :
)
��: ;
{
��< =
}
��> ?
public
�� +
LocalizationRequiredAttribute
�� ,
(
��, -
bool
��- 1
required
��2 :
)
��: ;
{
�� 	
Required
�� 
=
�� 
required
�� 
;
��  
}
�� 	
public
�� 
bool
�� 
Required
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Interface
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Class
��B G
|
��H I
AttributeTargets
��J Z
.
��Z [
Struct
��[ a
)
��a b
]
��b c
public
�� 

sealed
�� 
class
�� 2
$CannotApplyEqualityOperatorAttribute
�� <
:
��= >
	Attribute
��? H
{
��I J
}
��K L
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
,
��* +
AllowMultiple
��, 9
=
��: ;
true
��< @
)
��@ A
]
��A B
[
�� 
BaseTypeRequired
�� 
(
�� 
typeof
�� 
(
�� 
	Attribute
�� &
)
��& '
)
��' (
]
��( )
public
�� 

sealed
�� 
class
�� '
BaseTypeRequiredAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
�� 
public
�� '
BaseTypeRequiredAttribute
�� (
(
��( )
[
��) *
NotNull
��* 1
]
��1 2
Type
��3 7
baseType
��8 @
)
��@ A
{
�� 	
BaseType
�� 
=
�� 
baseType
�� 
;
��  
}
�� 	
[
�� 	
NotNull
��	 
]
�� 
public
�� 
Type
�� 
BaseType
�� &
{
��' (
get
��) ,
;
��, -
private
��. 5
set
��6 9
;
��9 :
}
��; <
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
All
��% (
)
��( )
]
��) *
public
�� 

sealed
�� 
class
�� %
UsedImplicitlyAttribute
�� /
:
��0 1
	Attribute
��2 ;
{
�� 
public
�� %
UsedImplicitlyAttribute
�� &
(
��& '
)
��' (
:
��
 
this
�� 
(
�� "
ImplicitUseKindFlags
�� %
.
��% &
Default
��& -
,
��- .$
ImplicitUseTargetFlags
��/ E
.
��E F
Default
��F M
)
��M N
{
��O P
}
��Q R
public
�� %
UsedImplicitlyAttribute
�� &
(
��& '"
ImplicitUseKindFlags
��' ;
useKindFlags
��< H
)
��H I
:
��
 
this
�� 
(
�� 
useKindFlags
�� 
,
�� $
ImplicitUseTargetFlags
�� 5
.
��5 6
Default
��6 =
)
��= >
{
��? @
}
��A B
public
�� %
UsedImplicitlyAttribute
�� &
(
��& '$
ImplicitUseTargetFlags
��' =
targetFlags
��> I
)
��I J
:
��
 
this
�� 
(
�� "
ImplicitUseKindFlags
�� %
.
��% &
Default
��& -
,
��- .
targetFlags
��/ :
)
��: ;
{
��< =
}
��> ?
public
�� %
UsedImplicitlyAttribute
�� &
(
��& '"
ImplicitUseKindFlags
��' ;
useKindFlags
��< H
,
��H I$
ImplicitUseTargetFlags
��J `
targetFlags
��a l
)
��l m
{
�� 	
UseKindFlags
�� 
=
�� 
useKindFlags
�� '
;
��' (
TargetFlags
�� 
=
�� 
targetFlags
�� %
;
��% &
}
�� 	
public
�� "
ImplicitUseKindFlags
�� #
UseKindFlags
��$ 0
{
��1 2
get
��3 6
;
��6 7
private
��8 ?
set
��@ C
;
��C D
}
��E F
public
�� $
ImplicitUseTargetFlags
�� %
TargetFlags
��& 1
{
��2 3
get
��4 7
;
��7 8
private
��9 @
set
��A D
;
��D E
}
��F G
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
|
��+ ,
AttributeTargets
��- =
.
��= >
GenericParameter
��> N
)
��N O
]
��O P
public
�� 

sealed
�� 
class
�� '
MeansImplicitUseAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
�� 
public
�� '
MeansImplicitUseAttribute
�� (
(
��( )
)
��) *
:
��
 
this
�� 
(
�� "
ImplicitUseKindFlags
�� %
.
��% &
Default
��& -
,
��- .$
ImplicitUseTargetFlags
��/ E
.
��E F
Default
��F M
)
��M N
{
��O P
}
��Q R
public
�� '
MeansImplicitUseAttribute
�� (
(
��( )"
ImplicitUseKindFlags
��) =
useKindFlags
��> J
)
��J K
:
��
 
this
�� 
(
�� 
useKindFlags
�� 
,
�� $
ImplicitUseTargetFlags
�� 5
.
��5 6
Default
��6 =
)
��= >
{
��? @
}
��A B
public
�� '
MeansImplicitUseAttribute
�� (
(
��( )$
ImplicitUseTargetFlags
��) ?
targetFlags
��@ K
)
��K L
:
��
 
this
�� 
(
�� "
ImplicitUseKindFlags
�� %
.
��% &
Default
��& -
,
��- .
targetFlags
��/ :
)
��: ;
{
��< =
}
��> ?
public
�� '
MeansImplicitUseAttribute
�� (
(
��( )"
ImplicitUseKindFlags
��) =
useKindFlags
��> J
,
��J K$
ImplicitUseTargetFlags
��L b
targetFlags
��c n
)
��n o
{
�� 	
UseKindFlags
�� 
=
�� 
useKindFlags
�� '
;
��' (
TargetFlags
�� 
=
�� 
targetFlags
�� %
;
��% &
}
�� 	
[
�� 	
UsedImplicitly
��	 
]
�� 
public
�� "
ImplicitUseKindFlags
��  4
UseKindFlags
��5 A
{
��B C
get
��D G
;
��G H
private
��I P
set
��Q T
;
��T U
}
��V W
[
�� 	
UsedImplicitly
��	 
]
�� 
public
�� $
ImplicitUseTargetFlags
��  6
TargetFlags
��7 B
{
��C D
get
��E H
;
��H I
private
��J Q
set
��R U
;
��U V
}
��W X
}
�� 
[
�� 
Flags
�� 

]
��
 
public
�� 

enum
�� "
ImplicitUseKindFlags
�� $
{
�� 
Default
�� 
=
�� 
Access
�� 
|
�� 
Assign
�� !
|
��" #7
)InstantiatedWithFixedConstructorSignature
��$ M
,
��M N
Access
�� 
=
�� 
$num
�� 
,
�� 
Assign
�� 
=
�� 
$num
�� 
,
�� 7
)InstantiatedWithFixedConstructorSignature
�� 1
=
��2 3
$num
��4 5
,
��5 65
'InstantiatedNoFixedConstructorSignature
�� /
=
��0 1
$num
��2 3
,
��3 4
}
�� 
[
�� 
Flags
�� 

]
��
 
public
�� 

enum
�� $
ImplicitUseTargetFlags
�� &
{
�� 
Default
�� 
=
�� 
Itself
�� 
,
�� 
Itself
�� 
=
�� 
$num
�� 
,
�� 
Members
�� 
=
�� 
$num
�� 
,
�� 
WithMembers
�� 
=
�� 
Itself
�� 
|
�� 
Members
�� &
}
�� 
[
�� 
MeansImplicitUse
�� 
(
�� $
ImplicitUseTargetFlags
�� ,
.
��, -
WithMembers
��- 8
)
��8 9
]
��9 :
public
�� 

sealed
�� 
class
��  
PublicAPIAttribute
�� *
:
��+ ,
	Attribute
��- 6
{
�� 
public
��  
PublicAPIAttribute
�� !
(
��! "
)
��" #
{
��$ %
}
��& '
public
��  
PublicAPIAttribute
�� !
(
��! "
[
��" #
NotNull
��# *
]
��* +
string
��, 2
comment
��3 :
)
��: ;
{
�� 	
Comment
�� 
=
�� 
comment
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Comment
�� 
{
�� 
get
��  #
;
��# $
private
��% ,
set
��- 0
;
��0 1
}
��2 3
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� $
InstantHandleAttribute
�� .
:
��/ 0
	Attribute
��1 :
{
��; <
}
��= >
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� 
PureAttribute
�� %
:
��& '
	Attribute
��( 1
{
��2 3
}
��4 5
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� $
PathReferenceAttribute
�� .
:
��/ 0
	Attribute
��1 :
{
�� 
public
�� $
PathReferenceAttribute
�� %
(
��% &
)
��& '
{
��( )
}
��* +
public
�� $
PathReferenceAttribute
�� %
(
��% &
[
��& '
PathReference
��' 4
]
��4 5
string
��6 <
basePath
��= E
)
��E F
{
�� 	
BasePath
�� 
=
�� 
basePath
�� 
;
��  
}
�� 	
public
�� 
string
�� 
BasePath
�� 
{
��  
get
��! $
;
��$ %
private
��& -
set
��. 1
;
��1 2
}
��3 4
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� %
SourceTemplateAttribute
�� /
:
��0 1
	Attribute
��2 ;
{
��< =
}
��> ?
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
,
��H I
AllowMultiple
��J W
=
��X Y
true
��Z ^
)
��^ _
]
��_ `
public
�� 

sealed
�� 
class
�� 
MacroAttribute
�� &
:
��' (
	Attribute
��) 2
{
�� 
public
�� 
string
�� 

Expression
��  
{
��! "
get
��# &
;
��& '
set
��( +
;
��+ ,
}
��- .
public
�� 
int
�� 
Editable
�� 
{
�� 
get
�� !
;
��! "
set
��# &
;
��& '
}
��( )
public
�� 
string
�� 
Target
�� 
{
�� 
get
�� "
;
��" #
set
��$ '
;
��' (
}
��) *
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� 5
'AspMvcAreaMasterLocationFormatAttribute
�� ?
:
��@ A
	Attribute
��B K
{
�� 
public
�� 5
'AspMvcAreaMasterLocationFormatAttribute
�� 6
(
��6 7
string
��7 =
format
��> D
)
��D E
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� :
,AspMvcAreaPartialViewLocationFormatAttribute
�� D
:
��E F
	Attribute
��G P
{
�� 
public
�� :
,AspMvcAreaPartialViewLocationFormatAttribute
�� ;
(
��; <
string
��< B
format
��C I
)
��I J
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� 3
%AspMvcAreaViewLocationFormatAttribute
�� =
:
��> ?
	Attribute
��@ I
{
�� 
public
�� 3
%AspMvcAreaViewLocationFormatAttribute
�� 4
(
��4 5
string
��5 ;
format
��< B
)
��B C
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� 1
#AspMvcMasterLocationFormatAttribute
�� ;
:
��< =
	Attribute
��> G
{
�� 
public
�� 1
#AspMvcMasterLocationFormatAttribute
�� 2
(
��2 3
string
��3 9
format
��: @
)
��@ A
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� 6
(AspMvcPartialViewLocationFormatAttribute
�� @
:
��A B
	Attribute
��C L
{
�� 
public
�� 6
(AspMvcPartialViewLocationFormatAttribute
�� 7
(
��7 8
string
��8 >
format
��? E
)
��E F
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� /
!AspMvcViewLocationFormatAttribute
�� 9
:
��: ;
	Attribute
��< E
{
�� 
public
�� /
!AspMvcViewLocationFormatAttribute
�� 0
(
��0 1
string
��1 7
format
��8 >
)
��> ?
{
�� 	
Format
�� 
=
�� 
format
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Format
�� 
{
�� 
get
�� "
;
��" #
private
��$ +
set
��, /
;
��/ 0
}
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
)
��H I
]
��I J
public
�� 

sealed
�� 
class
�� #
AspMvcActionAttribute
�� -
:
��. /
	Attribute
��0 9
{
�� 
public
�� #
AspMvcActionAttribute
�� $
(
��$ %
)
��% &
{
��' (
}
��) *
public
�� #
AspMvcActionAttribute
�� $
(
��$ %
string
��% +
anonymousProperty
��, =
)
��= >
{
�� 	
AnonymousProperty
�� 
=
�� 
anonymousProperty
��  1
;
��1 2
}
�� 	
public
�� 
string
�� 
AnonymousProperty
�� '
{
��( )
get
��* -
;
��- .
private
��/ 6
set
��7 :
;
��: ;
}
��< =
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� !
AspMvcAreaAttribute
�� +
:
��, -
	Attribute
��. 7
{
�� 
public
�� !
AspMvcAreaAttribute
�� "
(
��" #
)
��# $
{
��% &
}
��' (
public
�� !
AspMvcAreaAttribute
�� "
(
��" #
string
��# )
anonymousProperty
��* ;
)
��; <
{
�� 	
AnonymousProperty
�� 
=
�� 
anonymousProperty
��  1
;
��1 2
}
�� 	
public
�� 
string
�� 
AnonymousProperty
�� '
{
��( )
get
��* -
;
��- .
private
��/ 6
set
��7 :
;
��: ;
}
��< =
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
)
��H I
]
��I J
public
�� 

sealed
�� 
class
�� '
AspMvcControllerAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
�� 
public
�� '
AspMvcControllerAttribute
�� (
(
��( )
)
��) *
{
��+ ,
}
��- .
public
�� '
AspMvcControllerAttribute
�� (
(
��( )
string
��) /
anonymousProperty
��0 A
)
��A B
{
�� 	
AnonymousProperty
�� 
=
�� 
anonymousProperty
��  1
;
��1 2
}
�� 	
public
�� 
string
�� 
AnonymousProperty
�� '
{
��( )
get
��* -
;
��- .
private
��/ 6
set
��7 :
;
��: ;
}
��< =
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� #
AspMvcMasterAttribute
�� -
:
��. /
	Attribute
��0 9
{
��: ;
}
��< =
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� &
AspMvcModelTypeAttribute
�� 0
:
��1 2
	Attribute
��3 <
{
��= >
}
��? @
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
)
��H I
]
��I J
public
�� 

sealed
�� 
class
�� (
AspMvcPartialViewAttribute
�� 2
:
��3 4
	Attribute
��5 >
{
��? @
}
��A B
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
|
��+ ,
AttributeTargets
��- =
.
��= >
Method
��> D
)
��D E
]
��E F
public
�� 

sealed
�� 
class
�� -
AspMvcSupressViewErrorAttribute
�� 7
:
��8 9
	Attribute
��: C
{
��D E
}
��F G
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� ,
AspMvcDisplayTemplateAttribute
�� 6
:
��7 8
	Attribute
��9 B
{
��C D
}
��E F
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� +
AspMvcEditorTemplateAttribute
�� 5
:
��6 7
	Attribute
��8 A
{
��B C
}
��D E
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� %
AspMvcTemplateAttribute
�� /
:
��0 1
	Attribute
��2 ;
{
��< =
}
��> ?
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
)
��H I
]
��I J
public
�� 

sealed
�� 
class
�� !
AspMvcViewAttribute
�� +
:
��, -
	Attribute
��. 7
{
��8 9
}
��: ;
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Property
��B J
)
��J K
]
��K L
public
�� 

sealed
�� 
class
�� +
AspMvcActionSelectorAttribute
�� 5
:
��6 7
	Attribute
��8 A
{
��B C
}
��D E
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Property
��B J
|
��K L
AttributeTargets
��M ]
.
��] ^
Field
��^ c
)
��c d
]
��d e
public
�� 

sealed
�� 
class
�� ,
HtmlElementAttributesAttribute
�� 6
:
��7 8
	Attribute
��9 B
{
�� 
public
�� ,
HtmlElementAttributesAttribute
�� -
(
��- .
)
��. /
{
��0 1
}
��2 3
public
�� ,
HtmlElementAttributesAttribute
�� -
(
��- .
string
��. 4
name
��5 9
)
��9 :
{
�� 	
Name
�� 
=
�� 
name
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Name
�� 
{
�� 
get
��  
;
��  !
private
��" )
set
��* -
;
��- .
}
��/ 0
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Field
��B G
|
��H I
AttributeTargets
��J Z
.
��Z [
Property
��[ c
)
��c d
]
��d e
public
�� 

sealed
�� 
class
�� )
HtmlAttributeValueAttribute
�� 3
:
��4 5
	Attribute
��6 ?
{
�� 
public
�� )
HtmlAttributeValueAttribute
�� *
(
��* +
[
��+ ,
NotNull
��, 3
]
��3 4
string
��5 ;
name
��< @
)
��@ A
{
�� 	
Name
�� 
=
�� 
name
�� 
;
�� 
}
�� 	
[
�� 	
NotNull
��	 
]
�� 
public
�� 
string
�� 
Name
��  $
{
��% &
get
��' *
;
��* +
private
��, 3
set
��4 7
;
��7 8
}
��9 :
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
|
��/ 0
AttributeTargets
��1 A
.
��A B
Method
��B H
)
��H I
]
��I J
public
�� 

sealed
�� 
class
�� #
RazorSectionAttribute
�� -
:
��. /
	Attribute
��0 9
{
��: ;
}
��< =
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
|
��, -
AttributeTargets
��. >
.
��> ?
Constructor
��? J
|
��K L
AttributeTargets
��M ]
.
��] ^
Property
��^ f
)
��f g
]
��g h
public
�� 

sealed
�� 
class
�� '
CollectionAccessAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
�� 
public
�� '
CollectionAccessAttribute
�� (
(
��( )"
CollectionAccessType
��) ="
collectionAccessType
��> R
)
��R S
{
�� 	"
CollectionAccessType
��  
=
��! ""
collectionAccessType
��# 7
;
��7 8
}
�� 	
public
�� "
CollectionAccessType
�� #"
CollectionAccessType
��$ 8
{
��9 :
get
��; >
;
��> ?
private
��@ G
set
��H K
;
��K L
}
��M N
}
�� 
[
�� 
Flags
�� 

]
��
 
public
�� 

enum
�� "
CollectionAccessType
�� $
{
�� 
None
�� 
=
�� 
$num
�� 
,
�� 
Read
�� 
=
�� 
$num
�� 
,
�� #
ModifyExistingContent
�� 
=
�� 
$num
��  !
,
��! "
UpdatedContent
�� 
=
�� #
ModifyExistingContent
�� .
|
��/ 0
$num
��1 2
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� &
AssertionMethodAttribute
�� 0
:
��1 2
	Attribute
��3 <
{
��= >
}
��? @
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� )
AssertionConditionAttribute
�� 3
:
��4 5
	Attribute
��6 ?
{
�� 
public
�� )
AssertionConditionAttribute
�� *
(
��* +$
AssertionConditionType
��+ A
conditionType
��B O
)
��O P
{
�� 	
ConditionType
�� 
=
�� 
conditionType
�� )
;
��) *
}
�� 	
public
�� $
AssertionConditionType
�� %
ConditionType
��& 3
{
��4 5
get
��6 9
;
��9 :
private
��; B
set
��C F
;
��F G
}
��H I
}
�� 
public
�� 

enum
�� $
AssertionConditionType
�� &
{
�� 
IS_TRUE
�� 
=
�� 
$num
�� 
,
�� 
IS_FALSE
�� 
=
�� 
$num
�� 
,
�� 
IS_NULL
�� 
=
�� 
$num
�� 
,
�� 
IS_NOT_NULL
�� 
=
�� 
$num
�� 
,
�� 
}
�� 
[
�� 
Obsolete
�� 
(
�� 
$str
�� ;
)
��; <
]
��< =
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� (
TerminatesProgramAttribute
�� 2
:
��3 4
	Attribute
��5 >
{
��? @
}
��A B
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� !
LinqTunnelAttribute
�� +
:
��, -
	Attribute
��. 7
{
��8 9
}
��: ;
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� $
NoEnumerationAttribute
�� .
:
��/ 0
	Attribute
��1 :
{
��; <
}
��= >
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� #
RegexPatternAttribute
�� -
:
��. /
	Attribute
��0 9
{
��: ;
}
��< =
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
)
��* +
]
��+ ,
public
�� 

sealed
�� 
class
�� '
XamlItemsControlAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
��> ?
}
��@ A
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
)
��- .
]
��. /
public
�� 

sealed
�� 
class
�� 4
&XamlItemBindingOfItemsControlAttribute
�� >
:
��? @
	Attribute
��A J
{
��K L
}
��M N
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
,
��* +
AllowMultiple
��, 9
=
��: ;
true
��< @
)
��@ A
]
��A B
public
�� 

sealed
�� 
class
�� *
AspChildControlTypeAttribute
�� 4
:
��5 6
	Attribute
��7 @
{
�� 
public
�� *
AspChildControlTypeAttribute
�� +
(
��+ ,
string
��, 2
tagName
��3 :
,
��: ;
Type
��< @
controlType
��A L
)
��L M
{
�� 	
TagName
�� 
=
�� 
tagName
�� 
;
�� 
ControlType
�� 
=
�� 
controlType
�� %
;
��% &
}
�� 	
public
�� 
string
�� 
TagName
�� 
{
�� 
get
��  #
;
��# $
private
��% ,
set
��- 0
;
��0 1
}
��2 3
public
�� 
Type
�� 
ControlType
�� 
{
��  !
get
��" %
;
��% &
private
��' .
set
��/ 2
;
��2 3
}
��4 5
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
|
��. /
AttributeTargets
��0 @
.
��@ A
Method
��A G
)
��G H
]
��H I
public
�� 

sealed
�� 
class
�� #
AspDataFieldAttribute
�� -
:
��. /
	Attribute
��0 9
{
��: ;
}
��< =
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
|
��. /
AttributeTargets
��0 @
.
��@ A
Method
��A G
)
��G H
]
��H I
public
�� 

sealed
�� 
class
�� $
AspDataFieldsAttribute
�� .
:
��/ 0
	Attribute
��1 :
{
��; <
}
��= >
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
)
��- .
]
��. /
public
�� 

sealed
�� 
class
�� (
AspMethodPropertyAttribute
�� 2
:
��3 4
	Attribute
��5 >
{
��? @
}
��A B
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Class
��% *
,
��* +
AllowMultiple
��, 9
=
��: ;
true
��< @
)
��@ A
]
��A B
public
�� 

sealed
�� 
class
�� +
AspRequiredAttributeAttribute
�� 5
:
��6 7
	Attribute
��8 A
{
�� 
public
�� +
AspRequiredAttributeAttribute
�� ,
(
��, -
[
��- .
NotNull
��. 5
]
��5 6
string
��7 =
	attribute
��> G
)
��G H
{
�� 	
	Attribute
�� 
=
�� 
	attribute
�� !
;
��! "
}
�� 	
public
�� 
string
�� 
	Attribute
�� 
{
��  !
get
��" %
;
��% &
private
��' .
set
��/ 2
;
��2 3
}
��4 5
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
)
��- .
]
��. /
public
�� 

sealed
�� 
class
�� &
AspTypePropertyAttribute
�� 0
:
��1 2
	Attribute
��3 <
{
�� 
public
�� 
bool
�� )
CreateConstructorReferences
�� /
{
��0 1
get
��2 5
;
��5 6
private
��7 >
set
��? B
;
��B C
}
��D E
public
�� &
AspTypePropertyAttribute
�� '
(
��' (
bool
��( ,)
createConstructorReferences
��- H
)
��H I
{
�� 	)
CreateConstructorReferences
�� '
=
��( ))
createConstructorReferences
��* E
;
��E F
}
�� 	
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� +
RazorImportNamespaceAttribute
�� 5
:
��6 7
	Attribute
��8 A
{
�� 
public
�� +
RazorImportNamespaceAttribute
�� ,
(
��, -
string
��- 3
name
��4 8
)
��8 9
{
�� 	
Name
�� 
=
�� 
name
�� 
;
�� 
}
�� 	
public
�� 
string
�� 
Name
�� 
{
�� 
get
��  
;
��  !
private
��" )
set
��* -
;
��- .
}
��/ 0
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Assembly
��% -
,
��- .
AllowMultiple
��/ <
=
��= >
true
��? C
)
��C D
]
��D E
public
�� 

sealed
�� 
class
�� %
RazorInjectionAttribute
�� /
:
��0 1
	Attribute
��2 ;
{
�� 
public
�� %
RazorInjectionAttribute
�� &
(
��& '
string
��' -
type
��. 2
,
��2 3
string
��4 :
	fieldName
��; D
)
��D E
{
�� 	
Type
�� 
=
�� 
type
�� 
;
�� 
	FieldName
�� 
=
�� 
	fieldName
�� !
;
��! "
}
�� 	
public
�� 
string
�� 
Type
�� 
{
�� 
get
��  
;
��  !
private
��" )
set
��* -
;
��- .
}
��/ 0
public
�� 
string
�� 
	FieldName
�� 
{
��  !
get
��" %
;
��% &
private
��' .
set
��/ 2
;
��2 3
}
��4 5
}
�� 
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� (
RazorHelperCommonAttribute
�� 2
:
��3 4
	Attribute
��5 >
{
��? @
}
��A B
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Property
��% -
)
��- .
]
��. /
public
�� 

sealed
�� 
class
�� "
RazorLayoutAttribute
�� ,
:
��- .
	Attribute
��/ 8
{
��9 :
}
��; <
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� .
 RazorWriteLiteralMethodAttribute
�� 8
:
��9 :
	Attribute
��; D
{
��E F
}
��G H
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
Method
��% +
)
��+ ,
]
��, -
public
�� 

sealed
�� 
class
�� '
RazorWriteMethodAttribute
�� 1
:
��2 3
	Attribute
��4 =
{
��> ?
}
��@ A
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
	Parameter
��% .
)
��. /
]
��/ 0
public
�� 

sealed
�� 
class
�� 0
"RazorWriteMethodParameterAttribute
�� :
:
��; <
	Attribute
��= F
{
��G H
}
��I J
[
�� 
AttributeUsage
�� 
(
�� 
AttributeTargets
�� $
.
��$ %
All
��% (
)
��( )
]
��) *
public
�� 

sealed
�� 
class
�� 
	NoReorder
�� !
:
��" #
	Attribute
��$ -
{
��. /
}
��0 1
}�� �
SC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str $
)$ %
]% &
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
$str &
)& '
]' (
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
]##) *�9
DC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\DFe\DFe.Utils\Reflexao.cs
	namespace(( 	
DFe((
 
.(( 
Utils(( 
{)) 
public** 

static** 
class** 
Reflexao**  
{++ 
public33 
static33 
void33 
CopiarPropriedades33 -
<33- .
TDestino33. 6
,336 7
TOrigem338 ?
>33? @
(33@ A
this33A E
TDestino33F N
objetoDestino33O \
,33\ ]
TOrigem33^ e
objetoOrigem33f r
)33r s
where33t y
TDestino	33z �
:
33� �
class
33� �
where
33� �
TOrigem
33� �
:
33� �
class
33� �
{44 	
foreach55 
(55 
var55 
	attributo55 "
in55# %
objetoOrigem55& 2
.552 3
GetType553 :
(55: ;
)55; <
.55< =
GetProperties55= J
(55J K
)55K L
.55L M
Where55M R
(55R S
p55S T
=>55U W
p55X Y
.55Y Z
CanRead55Z a
)55a b
)55b c
{66 
var77 
propertyInfo77  
=77! "
objetoDestino77# 0
.770 1
GetType771 8
(778 9
)779 :
.77: ;
GetProperty77; F
(77F G
	attributo77G P
.77P Q
Name77Q U
,77U V
BindingFlags77W c
.77c d
Public77d j
|77k l
BindingFlags77m y
.77y z
Instance	77z �
)
77� �
;
77� �
if88 
(88 
propertyInfo88  
!=88! #
null88$ (
&&88) +
propertyInfo88, 8
.888 9
CanWrite889 A
)88A B
propertyInfo99  
.99  !
SetValue99! )
(99) *
objetoDestino99* 7
,997 8
	attributo999 B
.99B C
GetValue99C K
(99K L
objetoOrigem99L X
,99X Y
null99Z ^
)99^ _
,99_ `
null99a e
)99e f
;99f g
}:: 
};; 	
publicFF 
staticFF 
PropertyInfoFF " 
ObterPropriedadeInfoFF# 7
<FF7 8
TSourceFF8 ?
,FF? @
	TPropertyFFA J
>FFJ K
(FFK L
thisFFL P
TSourceFFQ X
sourceFFY _
,FF_ `

ExpressionFFa k
<FFk l
FuncFFl p
<FFp q
TSourceFFq x
,FFx y
	TProperty	FFz �
>
FF� �
>
FF� �
propertyLambda
FF� �
)
FF� �
{GG 	
varHH 
typeHH 
=HH 
typeofHH 
(HH 
TSourceHH %
)HH% &
;HH& '
varJJ 
memberJJ 
=JJ 
propertyLambdaJJ '
.JJ' (
BodyJJ( ,
asJJ- /
MemberExpressionJJ0 @
;JJ@ A
ifKK 
(KK 
memberKK 
==KK 
nullKK 
)KK 
throwLL 
newLL 
ArgumentExceptionLL +
(LL+ ,
stringLL, 2
.LL2 3
FormatLL3 9
(LL9 :
$strLL: {
,LL{ |
propertyLambda	LL} �
)
LL� �
)
LL� �
;
LL� �
varNN 
propInfoNN 
=NN 
memberNN !
.NN! "
MemberNN" (
asNN) +
PropertyInfoNN, 8
;NN8 9
ifOO 
(OO 
propInfoOO 
==OO 
nullOO  
)OO  !
throwPP 
newPP 
ArgumentExceptionPP +
(PP+ ,
stringPP, 2
.PP2 3
FormatPP3 9
(PP9 :
$strPP: z
,PPz {
propertyLambda	PP| �
)
PP� �
)
PP� �
;
PP� �
ifRR 
(RR 
propInfoRR 
.RR 
ReflectedTypeRR &
!=RR' )
nullRR* .
&&RR/ 1
(RR2 3
typeRR3 7
!=RR8 :
propInfoRR; C
.RRC D
ReflectedTypeRRD Q
&&RRR T
!RRU V
typeRRV Z
.RRZ [
IsSubclassOfRR[ g
(RRg h
propInfoRRh p
.RRp q
ReflectedTypeRRq ~
)RR~ 
)	RR �
)
RR� �
throwSS 
newSS 
ArgumentExceptionSS +
(SS+ ,
stringSS, 2
.SS2 3
FormatSS3 9
(SS9 :
$str	SS: �
,
SS� �
propertyLambda
SS� �
,
SS� �
type
SS� �
)
SS� �
)
SS� �
;
SS� �
returnUU 
propInfoUU 
;UU 
}VV 	
public^^ 
static^^ 

Dictionary^^  
<^^  !
string^^! '
,^^' (
object^^) /
>^^/ 0
LerPropriedades^^1 @
<^^@ A
T^^A B
>^^B C
(^^C D
this^^D H
T^^I J
objeto^^K Q
)^^Q R
where^^S X
T^^Y Z
:^^[ \
class^^] b
{__ 	
varaa 

dicionarioaa 
=aa 
newaa  

Dictionaryaa! +
<aa+ ,
stringaa, 2
,aa2 3
objectaa4 :
>aa: ;
(aa; <
)aa< =
;aa= >
foreachcc 
(cc 
varcc 
	attributocc "
incc# %
objetocc& ,
.cc, -
GetTypecc- 4
(cc4 5
)cc5 6
.cc6 7
GetPropertiescc7 D
(ccD E
)ccE F
)ccF G
{dd 
varee 
valueee 
=ee 
	attributoee %
.ee% &
GetValueee& .
(ee. /
objetoee/ 5
,ee5 6
nullee7 ;
)ee; <
;ee< =

dicionarioff 
.ff 
Addff 
(ff 
	attributoff (
.ff( )
Nameff) -
,ff- .
valueff/ 4
)ff4 5
;ff5 6
}gg 
returnii 

dicionarioii 
;ii 
}jj 	
publicss 
staticss 
Listss 
<ss 
stringss !
>ss! "%
ObterPropriedadesEmBrancoss# <
<ss< =
Tss= >
>ss> ?
(ss? @
thisss@ D
TssE F
objetossG M
)ssM N
{tt 	
returnuu 
(vv 
fromvv 
	attributovv 
invv  "
objetovv# )
.vv) *
GetTypevv* 1
(vv1 2
)vv2 3
.vv3 4
GetPropertiesvv4 A
(vvA B
)vvB C
letww 
valueww 
=ww 
	attributoww &
.ww& '
GetValueww' /
(ww/ 0
objetoww0 6
,ww6 7
nullww8 <
)ww< =
wherexx 
valuexx 
==xx 
nullxx  $
||xx% '
stringxx( .
.xx. /
IsNullOrEmptyxx/ <
(xx< =
valuexx= B
.xxB C
ToStringxxC K
(xxK L
)xxL M
)xxM N
selectyy 
	attributoyy !
.yy! "
Nameyy" &
)yy& '
.yy' (
ToListyy( .
(yy. /
)yy/ 0
;yy0 1
}zz 	
}{{ 
}|| 
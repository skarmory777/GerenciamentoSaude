µ
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
}LL ƒÉ
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
InvariantCultureIgnoreCase	\\{ ï
)
\\ï ñ
)
\\ñ ó
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
ÄÄ 
(
ÄÄ 
certificado
ÄÄ 
==
ÄÄ 
null
ÄÄ #
)
ÄÄ# $
throw
ÄÄ% *
new
ÄÄ+ .#
ArgumentNullException
ÄÄ/ D
(
ÄÄD E
$str
ÄÄE R
)
ÄÄR S
;
ÄÄS T
var
ÅÅ 
key
ÅÅ 
=
ÅÅ 
(
ÅÅ &
RSACryptoServiceProvider
ÅÅ /
)
ÅÅ/ 0
certificado
ÅÅ0 ;
.
ÅÅ; <

PrivateKey
ÅÅ< F
;
ÅÅF G
var
ÉÉ 
providerHandle
ÉÉ 
=
ÉÉ  
IntPtr
ÉÉ! '
.
ÉÉ' (
Zero
ÉÉ( ,
;
ÉÉ, -
var
ÑÑ 
	pinBuffer
ÑÑ 
=
ÑÑ 
Encoding
ÑÑ $
.
ÑÑ$ %
ASCII
ÑÑ% *
.
ÑÑ* +
GetBytes
ÑÑ+ 3
(
ÑÑ3 4
pin
ÑÑ4 7
)
ÑÑ7 8
;
ÑÑ8 9
MetodosNativos
ÜÜ 
.
ÜÜ 
Executar
ÜÜ #
(
ÜÜ# $
(
ÜÜ$ %
)
ÜÜ% &
=>
ÜÜ' )
MetodosNativos
ÜÜ* 8
.
ÜÜ8 9!
CryptAcquireContext
ÜÜ9 L
(
ÜÜL M
ref
ÜÜM P
providerHandle
ÜÜQ _
,
ÜÜ_ `
key
áá 
.
áá !
CspKeyContainerInfo
áá '
.
áá' (
KeyContainerName
áá( 8
,
áá8 9
key
àà 
.
àà !
CspKeyContainerInfo
àà '
.
àà' (
ProviderName
àà( 4
,
àà4 5
key
ââ 
.
ââ !
CspKeyContainerInfo
ââ '
.
ââ' (
ProviderType
ââ( 4
,
ââ4 5
MetodosNativos
ää 
.
ää 
CryptContextFlags
ää 0
.
ää0 1
Silent
ää1 7
)
ää7 8
)
ää8 9
;
ää9 :
MetodosNativos
ãã 
.
ãã 
Executar
ãã #
(
ãã# $
(
ãã$ %
)
ãã% &
=>
ãã' )
MetodosNativos
ãã* 8
.
ãã8 9
CryptSetProvParam
ãã9 J
(
ããJ K
providerHandle
ããK Y
,
ããY Z
MetodosNativos
åå 
.
åå 
CryptParameter
åå -
.
åå- .
KeyExchangePin
åå. <
,
åå< =
	pinBuffer
çç 
,
çç 
$num
çç 
)
çç 
)
çç 
;
çç 
MetodosNativos
éé 
.
éé 
Executar
éé #
(
éé# $
(
éé$ %
)
éé% &
=>
éé' )
MetodosNativos
éé* 8
.
éé8 9/
!CertSetCertificateContextProperty
éé9 Z
(
ééZ [
certificado
èè 
.
èè 
Handle
èè "
,
èè" #
MetodosNativos
êê 
.
êê !
CertificateProperty
êê 2
.
êê2 3"
CryptoProviderHandle
êê3 G
,
êêG H
$num
ëë 
,
ëë 
providerHandle
ëë !
)
ëë! "
)
ëë" #
;
ëë# $
}
íí 	
private
òò 
static
òò 
X509Certificate2
òò '#
ObterDadosCertificado
òò( =
(
òò= >%
ConfiguracaoCertificado
òò> U%
configuracaoCertificado
òòV m
)
òòm n
{
ôô 	
switch
öö 
(
öö %
configuracaoCertificado
öö +
.
öö+ ,
TipoCertificado
öö, ;
)
öö; <
{
õõ 
case
úú 
TipoCertificado
úú $
.
úú$ %
A1Repositorio
úú% 2
:
úú2 3
return
ùù  
ObterDoRepositorio
ùù -
(
ùù- .%
configuracaoCertificado
ùù. E
.
ùùE F
Serial
ùùF L
,
ùùL M
	OpenFlags
ùùN W
.
ùùW X

MaxAllowed
ùùX b
)
ùùb c
;
ùùc d
case
ûû 
TipoCertificado
ûû $
.
ûû$ %
	A1Arquivo
ûû% .
:
ûû. /
return
üü 
ObterDeArquivo
üü )
(
üü) *%
configuracaoCertificado
üü* A
.
üüA B
Arquivo
üüB I
,
üüI J%
configuracaoCertificado
üüK b
.
üüb c
Senha
üüc h
)
üüh i
;
üüi j
case
†† 
TipoCertificado
†† $
.
††$ %
A3
††% '
:
††' (
return
°° +
ObterDoRepositorioPassandoPin
°° 8
(
°°8 9%
configuracaoCertificado
°°9 P
.
°°P Q
Serial
°°Q W
,
°°W X%
configuracaoCertificado
°°Y p
.
°°p q
Senha
°°q v
)
°°v w
;
°°w x
default
¢¢ 
:
¢¢ 
throw
££ 
new
££ )
ArgumentOutOfRangeException
££ 9
(
££9 :
)
££: ;
;
££; <
}
§§ 
}
•• 	
public
≠≠ 
static
≠≠ 
X509Certificate2
≠≠ &'
ListareObterDoRepositorio
≠≠' @
(
≠≠@ A
)
≠≠A B
{
ÆÆ 	
var
ØØ 
store
ØØ 
=
ØØ 
ObterX509Store
ØØ &
(
ØØ& '
	OpenFlags
ØØ' 0
.
ØØ0 1
OpenExistingOnly
ØØ1 A
|
ØØB C
	OpenFlags
ØØD M
.
ØØM N
ReadOnly
ØØN V
)
ØØV W
;
ØØW X
var
∞∞ 

collection
∞∞ 
=
∞∞ 
store
∞∞ "
.
∞∞" #
Certificates
∞∞# /
;
∞∞/ 0
var
±± 
fcollection
±± 
=
±± 

collection
±± (
.
±±( )
Find
±±) -
(
±±- .
X509FindType
±±. :
.
±±: ;
FindByTimeValid
±±; J
,
±±J K
DateTime
±±L T
.
±±T U
Now
±±U X
,
±±X Y
true
±±Z ^
)
±±^ _
;
±±_ `
var
≤≤ 
scollection
≤≤ 
=
≤≤  
X509Certificate2UI
≤≤ 0
.
≤≤0 1"
SelectFromCollection
≤≤1 E
(
≤≤E F
fcollection
≤≤F Q
,
≤≤Q R
$str
≤≤S j
,
≤≤j k
$str≤≤l ï
,≤≤ï ñ
X509SelectionFlag
≥≥ !
.
≥≥! "
SingleSelection
≥≥" 1
)
≥≥1 2
;
≥≥2 3
if
µµ 
(
µµ 
scollection
µµ 
.
µµ 
Count
µµ !
==
µµ" $
$num
µµ% &
)
µµ& '
{
∂∂ 
throw
∑∑ 
new
∑∑ 
	Exception
∑∑ #
(
∑∑# $
$str
∑∑$ I
)
∑∑I J
;
∑∑J K
}
∏∏ 
store
∫∫ 
.
∫∫ 
Close
∫∫ 
(
∫∫ 
)
∫∫ 
;
∫∫ 
return
ªª 
scollection
ªª 
[
ªª 
$num
ªª  
]
ªª  !
;
ªª! "
}
ºº 	
public
≈≈ 
static
≈≈ 
X509Certificate2
≈≈ &
ObterCertificado
≈≈' 7
(
≈≈7 8%
ConfiguracaoCertificado
≈≈8 O%
configuracaoCertificado
≈≈P g
)
≈≈g h
{
∆∆ 	
if
«« 
(
«« 
!
«« %
configuracaoCertificado
«« (
.
««( ) 
ManterDadosEmCache
««) ;
)
««; <
return
»» #
ObterDadosCertificado
»» ,
(
»», -%
configuracaoCertificado
»»- D
)
»»D E
;
»»E F
if
…… 
(
…… 
_certificado
…… 
!=
…… 
null
……  $
)
……$ %
return
   
_certificado
   #
;
  # $
_certificado
ÀÀ 
=
ÀÀ #
ObterDadosCertificado
ÀÀ 0
(
ÀÀ0 1%
configuracaoCertificado
ÀÀ1 H
)
ÀÀH I
;
ÀÀI J
return
ÃÃ 
_certificado
ÃÃ 
;
ÃÃ  
}
ÕÕ 	
}
ŒŒ 
internal
–– 
static
–– 
class
–– 
MetodosNativos
–– (
{
—— 
internal
““ 
enum
““ 
CryptContextFlags
““ '
{
”” 	
None
‘‘ 
=
‘‘ 
$num
‘‘ 
,
‘‘ 
Silent
’’ 
=
’’ 
$num
’’ 
}
÷÷ 	
internal
ÿÿ 
enum
ÿÿ !
CertificateProperty
ÿÿ )
{
ŸŸ 	
None
⁄⁄ 
=
⁄⁄ 
$num
⁄⁄ 
,
⁄⁄ "
CryptoProviderHandle
€€  
=
€€! "
$num
€€# &
}
‹‹ 	
internal
ﬁﬁ 
enum
ﬁﬁ 
CryptParameter
ﬁﬁ $
{
ﬂﬂ 	
None
‡‡ 
=
‡‡ 
$num
‡‡ 
,
‡‡ 
KeyExchangePin
·· 
=
·· 
$num
·· !
}
‚‚ 	
[
‰‰ 	
	DllImport
‰‰	 
(
‰‰ 
$str
‰‰ !
,
‰‰! "
CharSet
‰‰# *
=
‰‰+ ,
CharSet
‰‰- 4
.
‰‰4 5
Auto
‰‰5 9
,
‰‰9 :
SetLastError
‰‰; G
=
‰‰H I
true
‰‰J N
)
‰‰N O
]
‰‰O P
public
ÂÂ 
static
ÂÂ 
extern
ÂÂ 
bool
ÂÂ !!
CryptAcquireContext
ÂÂ" 5
(
ÂÂ5 6
ref
ÊÊ 
IntPtr
ÊÊ 
hProv
ÊÊ 
,
ÊÊ 
string
ÁÁ 
containerName
ÁÁ  
,
ÁÁ  !
string
ËË 
providerName
ËË 
,
ËË  
int
ÈÈ 
providerType
ÈÈ 
,
ÈÈ 
CryptContextFlags
ÍÍ 
flags
ÍÍ #
)
ÎÎ 
;
ÎÎ 
[
ÌÌ 	
	DllImport
ÌÌ	 
(
ÌÌ 
$str
ÌÌ !
,
ÌÌ! "
SetLastError
ÌÌ# /
=
ÌÌ0 1
true
ÌÌ2 6
,
ÌÌ6 7
CharSet
ÌÌ8 ?
=
ÌÌ@ A
CharSet
ÌÌB I
.
ÌÌI J
Auto
ÌÌJ N
)
ÌÌN O
]
ÌÌO P
public
ÓÓ 
static
ÓÓ 
extern
ÓÓ 
bool
ÓÓ !
CryptSetProvParam
ÓÓ" 3
(
ÓÓ3 4
IntPtr
ÔÔ 
hProv
ÔÔ 
,
ÔÔ 
CryptParameter
 
dwParam
 "
,
" #
[
ÒÒ 
In
ÒÒ 
]
ÒÒ 
byte
ÒÒ 
[
ÒÒ 
]
ÒÒ 
pbData
ÒÒ 
,
ÒÒ 
uint
ÚÚ 
dwFlags
ÚÚ 
)
ÚÚ 
;
ÚÚ 
[
ÙÙ 	
	DllImport
ÙÙ	 
(
ÙÙ 
$str
ÙÙ  
,
ÙÙ  !
SetLastError
ÙÙ" .
=
ÙÙ/ 0
true
ÙÙ1 5
)
ÙÙ5 6
]
ÙÙ6 7
internal
ıı 
static
ıı 
extern
ıı 
bool
ıı #/
!CertSetCertificateContextProperty
ıı$ E
(
ııE F
IntPtr
ˆˆ 
pCertContext
ˆˆ 
,
ˆˆ  !
CertificateProperty
˜˜ 

propertyId
˜˜  *
,
˜˜* +
uint
¯¯ 
dwFlags
¯¯ 
,
¯¯ 
IntPtr
˘˘ 
pvData
˘˘ 
)
˙˙ 
;
˙˙ 
public
¸¸ 
static
¸¸ 
void
¸¸ 
Executar
¸¸ #
(
¸¸# $
Func
¸¸$ (
<
¸¸( )
bool
¸¸) -
>
¸¸- .
action
¸¸/ 5
)
¸¸5 6
{
˝˝ 	
if
˛˛ 
(
˛˛ 
!
˛˛ 
action
˛˛ 
(
˛˛ 
)
˛˛ 
)
˛˛ 
{
ˇˇ 
throw
ÄÄ 
new
ÄÄ 
System
ÄÄ  
.
ÄÄ  !
ComponentModel
ÄÄ! /
.
ÄÄ/ 0
Win32Exception
ÄÄ0 >
(
ÄÄ> ?
Marshal
ÄÄ? F
.
ÄÄF G
GetLastWin32Error
ÄÄG X
(
ÄÄX Y
)
ÄÄY Z
)
ÄÄZ [
;
ÄÄ[ \
}
ÅÅ 
}
ÇÇ 	
}
ÉÉ 
}ÑÑ òA
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
this	TT} Å
.
TTÅ Ç"
ObterPropriedadeInfo
TTÇ ñ
(
TTñ ó
c
TTó ò
=>
TTô õ
c
TTú ù
.
TTù û
Serial
TTû §
)
TT§ •
.
TT• ¶
Name
TT¶ ™
)
TT™ ´
)
TT´ ¨
;
TT¨ ≠
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
this	gg} Å
.
ggÅ Ç"
ObterPropriedadeInfo
ggÇ ñ
(
ggñ ó
c
ggó ò
=>
ggô õ
c
ggú ù
.
ggù û
Arquivo
ggû •
)
gg• ¶
.
gg¶ ß
Name
ggß ´
)
gg´ ¨
)
gg¨ ≠
;
gg≠ Æ
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
this	{{} Å
.
{{Å Ç"
ObterPropriedadeInfo
{{Ç ñ
(
{{ñ ó
c
{{ó ò
=>
{{ô õ
c
{{ú ù
.
{{ù û
Senha
{{û £
)
{{£ §
.
{{§ •
Name
{{• ©
)
{{© ™
)
{{™ ´
;
{{´ ¨
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
ÄÄ 	
public
ÜÜ 
bool
ÜÜ  
ManterDadosEmCache
ÜÜ &
{
ÜÜ' (
get
ÜÜ) ,
;
ÜÜ, -
set
ÜÜ. 1
;
ÜÜ1 2
}
ÜÜ3 4
public
àà 
event
àà )
PropertyChangedEventHandler
àà 0
PropertyChanged
àà1 @
;
àà@ A
[
ää 	,
NotifyPropertyChangedInvocator
ää	 '
]
ää' (
	protected
ãã 
virtual
ãã 
void
ãã 
OnPropertyChanged
ãã 0
(
ãã0 1
string
ãã1 7
propertyName
ãã8 D
)
ããD E
{
åå 	
if
çç 
(
çç 
PropertyChanged
çç 
!=
çç  "
null
çç# '
)
çç' (
PropertyChanged
çç) 8
.
çç8 9
Invoke
çç9 ?
(
çç? @
this
çç@ D
,
ççD E
new
ççF I&
PropertyChangedEventArgs
ççJ b
(
ççb c
propertyName
ççc o
)
çço p
)
ççp q
;
ççq r
}
éé 	
}
èè 
}êê Ã
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
}YY Ê
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
}&& Ω^
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
ÄÄ 
.
ÄÄ 
	WriteLine
ÄÄ 
(
ÄÄ 
xml
ÄÄ !
)
ÄÄ! "
;
ÄÄ" #
stw
ÅÅ 
.
ÅÅ 
Close
ÅÅ 
(
ÅÅ 
)
ÅÅ 
;
ÅÅ 
}
ÇÇ 
catch
ÉÉ 
(
ÉÉ 
	Exception
ÉÉ 
)
ÉÉ 
{
ÑÑ 
throw
ÖÖ 
new
ÖÖ 
	Exception
ÖÖ #
(
ÖÖ# $
$str
ÖÖ$ G
+
ÖÖH I
arquivo
ÖÖJ Q
+
ÖÖR S
$str
ÖÖT W
)
ÖÖW X
;
ÖÖX Y
}
ÜÜ 
}
áá 	
public
ââ 
static
ââ 
void
ââ +
SalvarStringXmlParaArquivoXml
ââ 8
(
ââ8 9
string
ââ9 ?
xml
ââ@ C
,
ââC D
string
ââE K
arquivo
ââL S
)
ââS T
{
ää 	
var
ãã 
dir
ãã 
=
ãã 
Path
ãã 
.
ãã 
GetDirectoryName
ãã +
(
ãã+ ,
arquivo
ãã, 3
)
ãã3 4
;
ãã4 5
if
åå 
(
åå 
dir
åå 
!=
åå 
null
åå 
&&
åå 
!
åå  
	Directory
åå  )
.
åå) *
Exists
åå* 0
(
åå0 1
dir
åå1 4
)
åå4 5
)
åå5 6
{
çç 
throw
éé 
new
éé (
DirectoryNotFoundException
éé 4
(
éé4 5
$str
éé5 A
+
ééB C
dir
ééD G
+
ééH I
$str
ééJ \
)
éé\ ]
;
éé] ^
}
èè 
try
ëë 
{
íí 
var
ìì 
stw
ìì 
=
ìì 
new
ìì 
StreamWriter
ìì *
(
ìì* +
arquivo
ìì+ 2
)
ìì2 3
;
ìì3 4
stw
îî 
.
îî 
	WriteLine
îî 
(
îî 
xml
îî !
)
îî! "
;
îî" #
stw
ïï 
.
ïï 
Close
ïï 
(
ïï 
)
ïï 
;
ïï 
}
ññ 
catch
óó 
(
óó 
	Exception
óó 
)
óó 
{
òò 
throw
ôô 
new
ôô 
	Exception
ôô #
(
ôô# $
$str
ôô$ G
+
ôôH I
arquivo
ôôJ Q
+
ôôR S
$str
ôôT W
)
ôôW X
;
ôôX Y
}
öö 
}
õõ 	
public
§§ 
static
§§ 
string
§§ #
ObterNodeDeArquivoXml
§§ 2
(
§§2 3
string
§§3 9

nomeDoNode
§§: D
,
§§D E
string
§§F L

arquivoXml
§§M W
)
§§W X
{
•• 	
var
¶¶ 
xmlDoc
¶¶ 
=
¶¶ 
	XDocument
¶¶ "
.
¶¶" #
Load
¶¶# '
(
¶¶' (

arquivoXml
¶¶( 2
)
¶¶2 3
;
¶¶3 4
var
ßß 
	xmlString
ßß 
=
ßß 
(
ßß 
from
ßß !
d
ßß" #
in
ßß$ &
xmlDoc
ßß' -
.
ßß- .
Descendants
ßß. 9
(
ßß9 :
)
ßß: ;
where
®® "
d
®®# $
.
®®$ %
Name
®®% )
.
®®) *
	LocalName
®®* 3
==
®®4 6

nomeDoNode
®®7 A
select
©© #
d
©©$ %
)
©©% &
.
©©& '
FirstOrDefault
©©' 5
(
©©5 6
)
©©6 7
;
©©7 8
if
´´ 
(
´´ 
	xmlString
´´ 
==
´´ 
null
´´ !
)
´´! "
throw
¨¨ 
new
¨¨ 
	Exception
¨¨ #
(
¨¨# $
String
¨¨$ *
.
¨¨* +
Format
¨¨+ 1
(
¨¨1 2
$str
¨¨2 `
,
¨¨` a

nomeDoNode
¨¨b l
,
¨¨l m

arquivoXml
¨¨n x
)
¨¨x y
)
¨¨y z
;
¨¨z {
return
≠≠ 
	xmlString
≠≠ 
.
≠≠ 
ToString
≠≠ %
(
≠≠% &
)
≠≠& '
;
≠≠' (
}
ÆÆ 	
public
∑∑ 
static
∑∑ 
string
∑∑ "
ObterNodeDeStringXml
∑∑ 1
(
∑∑1 2
string
∑∑2 8

nomeDoNode
∑∑9 C
,
∑∑C D
string
∑∑E K
	stringXml
∑∑L U
)
∑∑U V
{
∏∏ 	
var
ππ 
s
ππ 
=
ππ 
	stringXml
ππ 
;
ππ 
var
∫∫ 
xmlDoc
∫∫ 
=
∫∫ 
	XDocument
∫∫ "
.
∫∫" #
Parse
∫∫# (
(
∫∫( )
s
∫∫) *
)
∫∫* +
;
∫∫+ ,
var
ªª 
	xmlString
ªª 
=
ªª 
(
ªª 
from
ªª !
d
ªª" #
in
ªª$ &
xmlDoc
ªª' -
.
ªª- .
Descendants
ªª. 9
(
ªª9 :
)
ªª: ;
where
ºº "
d
ºº# $
.
ºº$ %
Name
ºº% )
.
ºº) *
	LocalName
ºº* 3
==
ºº4 6

nomeDoNode
ºº7 A
select
ΩΩ #
d
ΩΩ$ %
)
ΩΩ% &
.
ΩΩ& '
FirstOrDefault
ΩΩ' 5
(
ΩΩ5 6
)
ΩΩ6 7
;
ΩΩ7 8
if
øø 
(
øø 
	xmlString
øø 
==
øø 
null
øø !
)
øø! "
throw
¿¿ 
new
¿¿ 
	Exception
¿¿ #
(
¿¿# $
String
¿¿$ *
.
¿¿* +
Format
¿¿+ 1
(
¿¿1 2
$str
¿¿2 X
,
¿¿X Y

nomeDoNode
¿¿Z d
)
¿¿d e
)
¿¿e f
;
¿¿f g
return
¡¡ 
	xmlString
¡¡ 
.
¡¡ 
ToString
¡¡ %
(
¡¡% &
)
¡¡& '
;
¡¡' (
}
¬¬ 	
}
√√ 
}ƒƒ ÏS
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
ModeloDocumento	99s Ç
modelo
99É â
,
99â ä
int
99ã é
serie
99è î
,
99î ï
long
99ñ ö
numero
99õ °
,
99° ¢
int
99£ ¶
tipoEmissao
99ß ≤
,
99≤ ≥
int
99¥ ∑
cNf
99∏ ª
)
99ª º
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
ÄÄ 
cnpj
ÄÄ 
=
ÄÄ 
chaveNfe
ÄÄ 
.
ÄÄ  
	Substring
ÄÄ  )
(
ÄÄ) *
$num
ÄÄ* +
,
ÄÄ+ ,
$num
ÄÄ- /
)
ÄÄ/ 0
;
ÄÄ0 1
ModeloDocumento
ÅÅ 
modelo
ÅÅ "
;
ÅÅ" #
Enum
ÇÇ 
.
ÇÇ 
TryParse
ÇÇ 
(
ÇÇ 
chaveNfe
ÇÇ "
.
ÇÇ" #
	Substring
ÇÇ# ,
(
ÇÇ, -
$num
ÇÇ- /
,
ÇÇ/ 0
$num
ÇÇ1 2
)
ÇÇ2 3
,
ÇÇ3 4
out
ÇÇ5 8
modelo
ÇÇ9 ?
)
ÇÇ? @
;
ÇÇ@ A
var
ÉÉ 
serie
ÉÉ 
=
ÉÉ 
int
ÉÉ 
.
ÉÉ 
Parse
ÉÉ !
(
ÉÉ! "
chaveNfe
ÉÉ" *
.
ÉÉ* +
	Substring
ÉÉ+ 4
(
ÉÉ4 5
$num
ÉÉ5 7
,
ÉÉ7 8
$num
ÉÉ9 :
)
ÉÉ: ;
)
ÉÉ; <
;
ÉÉ< =
var
ÑÑ 
	numeroNfe
ÑÑ 
=
ÑÑ 
long
ÑÑ  
.
ÑÑ  !
Parse
ÑÑ! &
(
ÑÑ& '
chaveNfe
ÑÑ' /
.
ÑÑ/ 0
	Substring
ÑÑ0 9
(
ÑÑ9 :
$num
ÑÑ: <
,
ÑÑ< =
$num
ÑÑ> ?
)
ÑÑ? @
)
ÑÑ@ A
;
ÑÑA B
var
ÖÖ 
formaEmissao
ÖÖ 
=
ÖÖ 
int
ÖÖ "
.
ÖÖ" #
Parse
ÖÖ# (
(
ÖÖ( )
chaveNfe
ÖÖ) 1
.
ÖÖ1 2
	Substring
ÖÖ2 ;
(
ÖÖ; <
$num
ÖÖ< >
,
ÖÖ> ?
$num
ÖÖ@ A
)
ÖÖA B
)
ÖÖB C
;
ÖÖC D
var
ÜÜ 
codigoNumerico
ÜÜ 
=
ÜÜ  
int
ÜÜ! $
.
ÜÜ$ %
Parse
ÜÜ% *
(
ÜÜ* +
chaveNfe
ÜÜ+ 3
.
ÜÜ3 4
	Substring
ÜÜ4 =
(
ÜÜ= >
$num
ÜÜ> @
,
ÜÜ@ A
$num
ÜÜB C
)
ÜÜC D
)
ÜÜD E
;
ÜÜE F
var
áá 
digitoVerificador
áá !
=
áá" #
chaveNfe
áá$ ,
.
áá, -
	Substring
áá- 6
(
áá6 7
$num
áá7 9
,
áá9 :
$num
áá; <
)
áá< =
;
áá= >
var
ââ 

gerarChave
ââ 
=
ââ 

ObterChave
ââ '
(
ââ' (
codigo
ââ( .
,
ââ. /
anoEMesData
ââ0 ;
,
ââ; <
cnpj
ââ= A
,
ââA B
modelo
ââC I
,
ââI J
serie
ââK P
,
ââP Q
	numeroNfe
ââR [
,
ââ[ \
formaEmissao
ââ] i
,
ââi j
codigoNumerico
ââk y
)
âây z
;
ââz {
return
ãã 
digitoVerificador
ãã $
.
ãã$ %
Equals
ãã% +
(
ãã+ ,

gerarChave
ãã, 6
.
ãã6 7
DigitoVerificador
ãã7 H
.
ããH I
ToString
ããI Q
(
ããQ R
)
ããR S
)
ããS T
;
ããT U
}
åå 	
}
çç 
public
íí 

class
íí 
DadosChaveFiscal
íí !
{
ìì 
public
îî 
DadosChaveFiscal
îî 
(
îî  
string
îî  &
chave
îî' ,
,
îî, -
byte
îî. 2
digitoVerificador
îî3 D
)
îîD E
{
ïï 	
Chave
ññ 
=
ññ 
chave
ññ 
;
ññ 
DigitoVerificador
óó 
=
óó 
digitoVerificador
óó  1
;
óó1 2
}
òò 	
public
ùù 
string
ùù 
Chave
ùù 
{
ùù 
get
ùù !
;
ùù! "
private
ùù# *
set
ùù+ .
;
ùù. /
}
ùù0 1
public
¢¢ 
byte
¢¢ 
DigitoVerificador
¢¢ %
{
¢¢& '
get
¢¢( +
;
¢¢+ ,
private
¢¢- 4
set
¢¢5 8
;
¢¢8 9
}
¢¢: ;
}
££ 
}§§ €É
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
úú 
AttributeUsage
úú 
(
úú 
AttributeTargets
úú $
.
úú$ %
Method
úú% +
)
úú+ ,
]
úú, -
public
ùù 

sealed
ùù 
class
ùù 5
'NotifyPropertyChangedInvocatorAttribute
ùù ?
:
ùù@ A
	Attribute
ùùB K
{
ûû 
public
üü 5
'NotifyPropertyChangedInvocatorAttribute
üü 6
(
üü6 7
)
üü7 8
{
üü9 :
}
üü; <
public
†† 5
'NotifyPropertyChangedInvocatorAttribute
†† 6
(
††6 7
string
††7 =
parameterName
††> K
)
††K L
{
°° 	
ParameterName
¢¢ 
=
¢¢ 
parameterName
¢¢ )
;
¢¢) *
}
££ 	
public
•• 
string
•• 
ParameterName
•• #
{
••$ %
get
••& )
;
••) *
private
••+ 2
set
••3 6
;
••6 7
}
••8 9
}
¶¶ 
[
”” 
AttributeUsage
”” 
(
”” 
AttributeTargets
”” $
.
””$ %
Method
””% +
,
””+ ,
AllowMultiple
””- :
=
””; <
true
””= A
)
””A B
]
””B C
public
‘‘ 

sealed
‘‘ 
class
‘‘ )
ContractAnnotationAttribute
‘‘ 3
:
‘‘4 5
	Attribute
‘‘6 ?
{
’’ 
public
÷÷ )
ContractAnnotationAttribute
÷÷ *
(
÷÷* +
[
÷÷+ ,
NotNull
÷÷, 3
]
÷÷3 4
string
÷÷5 ;
contract
÷÷< D
)
÷÷D E
:
◊◊
 
this
◊◊ 
(
◊◊ 
contract
◊◊ 
,
◊◊ 
false
◊◊  
)
◊◊  !
{
◊◊" #
}
◊◊$ %
public
ŸŸ )
ContractAnnotationAttribute
ŸŸ *
(
ŸŸ* +
[
ŸŸ+ ,
NotNull
ŸŸ, 3
]
ŸŸ3 4
string
ŸŸ5 ;
contract
ŸŸ< D
,
ŸŸD E
bool
ŸŸF J
forceFullStates
ŸŸK Z
)
ŸŸZ [
{
⁄⁄ 	
Contract
€€ 
=
€€ 
contract
€€ 
;
€€  
ForceFullStates
‹‹ 
=
‹‹ 
forceFullStates
‹‹ -
;
‹‹- .
}
›› 	
public
ﬂﬂ 
string
ﬂﬂ 
Contract
ﬂﬂ 
{
ﬂﬂ  
get
ﬂﬂ! $
;
ﬂﬂ$ %
private
ﬂﬂ& -
set
ﬂﬂ. 1
;
ﬂﬂ1 2
}
ﬂﬂ3 4
public
‡‡ 
bool
‡‡ 
ForceFullStates
‡‡ #
{
‡‡$ %
get
‡‡& )
;
‡‡) *
private
‡‡+ 2
set
‡‡3 6
;
‡‡6 7
}
‡‡8 9
}
·· 
[
ÏÏ 
AttributeUsage
ÏÏ 
(
ÏÏ 
AttributeTargets
ÏÏ $
.
ÏÏ$ %
All
ÏÏ% (
)
ÏÏ( )
]
ÏÏ) *
public
ÌÌ 

sealed
ÌÌ 
class
ÌÌ +
LocalizationRequiredAttribute
ÌÌ 5
:
ÌÌ6 7
	Attribute
ÌÌ8 A
{
ÓÓ 
public
ÔÔ +
LocalizationRequiredAttribute
ÔÔ ,
(
ÔÔ, -
)
ÔÔ- .
:
ÔÔ/ 0
this
ÔÔ1 5
(
ÔÔ5 6
true
ÔÔ6 :
)
ÔÔ: ;
{
ÔÔ< =
}
ÔÔ> ?
public
 +
LocalizationRequiredAttribute
 ,
(
, -
bool
- 1
required
2 :
)
: ;
{
ÒÒ 	
Required
ÚÚ 
=
ÚÚ 
required
ÚÚ 
;
ÚÚ  
}
ÛÛ 	
public
ıı 
bool
ıı 
Required
ıı 
{
ıı 
get
ıı "
;
ıı" #
private
ıı$ +
set
ıı, /
;
ıı/ 0
}
ıı1 2
}
ˆˆ 
[
ãã 
AttributeUsage
ãã 
(
ãã 
AttributeTargets
ãã $
.
ãã$ %
	Interface
ãã% .
|
ãã/ 0
AttributeTargets
ãã1 A
.
ããA B
Class
ããB G
|
ããH I
AttributeTargets
ããJ Z
.
ããZ [
Struct
ãã[ a
)
ããa b
]
ããb c
public
åå 

sealed
åå 
class
åå 2
$CannotApplyEqualityOperatorAttribute
åå <
:
åå= >
	Attribute
åå? H
{
ååI J
}
ååK L
[
òò 
AttributeUsage
òò 
(
òò 
AttributeTargets
òò $
.
òò$ %
Class
òò% *
,
òò* +
AllowMultiple
òò, 9
=
òò: ;
true
òò< @
)
òò@ A
]
òòA B
[
ôô 
BaseTypeRequired
ôô 
(
ôô 
typeof
ôô 
(
ôô 
	Attribute
ôô &
)
ôô& '
)
ôô' (
]
ôô( )
public
öö 

sealed
öö 
class
öö '
BaseTypeRequiredAttribute
öö 1
:
öö2 3
	Attribute
öö4 =
{
õõ 
public
úú '
BaseTypeRequiredAttribute
úú (
(
úú( )
[
úú) *
NotNull
úú* 1
]
úú1 2
Type
úú3 7
baseType
úú8 @
)
úú@ A
{
ùù 	
BaseType
ûû 
=
ûû 
baseType
ûû 
;
ûû  
}
üü 	
[
°° 	
NotNull
°°	 
]
°° 
public
°° 
Type
°° 
BaseType
°° &
{
°°' (
get
°°) ,
;
°°, -
private
°°. 5
set
°°6 9
;
°°9 :
}
°°; <
}
¢¢ 
[
®® 
AttributeUsage
®® 
(
®® 
AttributeTargets
®® $
.
®®$ %
All
®®% (
)
®®( )
]
®®) *
public
©© 

sealed
©© 
class
©© %
UsedImplicitlyAttribute
©© /
:
©©0 1
	Attribute
©©2 ;
{
™™ 
public
´´ %
UsedImplicitlyAttribute
´´ &
(
´´& '
)
´´' (
:
¨¨
 
this
¨¨ 
(
¨¨ "
ImplicitUseKindFlags
¨¨ %
.
¨¨% &
Default
¨¨& -
,
¨¨- .$
ImplicitUseTargetFlags
¨¨/ E
.
¨¨E F
Default
¨¨F M
)
¨¨M N
{
¨¨O P
}
¨¨Q R
public
ÆÆ %
UsedImplicitlyAttribute
ÆÆ &
(
ÆÆ& '"
ImplicitUseKindFlags
ÆÆ' ;
useKindFlags
ÆÆ< H
)
ÆÆH I
:
ØØ
 
this
ØØ 
(
ØØ 
useKindFlags
ØØ 
,
ØØ $
ImplicitUseTargetFlags
ØØ 5
.
ØØ5 6
Default
ØØ6 =
)
ØØ= >
{
ØØ? @
}
ØØA B
public
±± %
UsedImplicitlyAttribute
±± &
(
±±& '$
ImplicitUseTargetFlags
±±' =
targetFlags
±±> I
)
±±I J
:
≤≤
 
this
≤≤ 
(
≤≤ "
ImplicitUseKindFlags
≤≤ %
.
≤≤% &
Default
≤≤& -
,
≤≤- .
targetFlags
≤≤/ :
)
≤≤: ;
{
≤≤< =
}
≤≤> ?
public
¥¥ %
UsedImplicitlyAttribute
¥¥ &
(
¥¥& '"
ImplicitUseKindFlags
¥¥' ;
useKindFlags
¥¥< H
,
¥¥H I$
ImplicitUseTargetFlags
¥¥J `
targetFlags
¥¥a l
)
¥¥l m
{
µµ 	
UseKindFlags
∂∂ 
=
∂∂ 
useKindFlags
∂∂ '
;
∂∂' (
TargetFlags
∑∑ 
=
∑∑ 
targetFlags
∑∑ %
;
∑∑% &
}
∏∏ 	
public
∫∫ "
ImplicitUseKindFlags
∫∫ #
UseKindFlags
∫∫$ 0
{
∫∫1 2
get
∫∫3 6
;
∫∫6 7
private
∫∫8 ?
set
∫∫@ C
;
∫∫C D
}
∫∫E F
public
ªª $
ImplicitUseTargetFlags
ªª %
TargetFlags
ªª& 1
{
ªª2 3
get
ªª4 7
;
ªª7 8
private
ªª9 @
set
ªªA D
;
ªªD E
}
ªªF G
}
ºº 
[
¬¬ 
AttributeUsage
¬¬ 
(
¬¬ 
AttributeTargets
¬¬ $
.
¬¬$ %
Class
¬¬% *
|
¬¬+ ,
AttributeTargets
¬¬- =
.
¬¬= >
GenericParameter
¬¬> N
)
¬¬N O
]
¬¬O P
public
√√ 

sealed
√√ 
class
√√ '
MeansImplicitUseAttribute
√√ 1
:
√√2 3
	Attribute
√√4 =
{
ƒƒ 
public
≈≈ '
MeansImplicitUseAttribute
≈≈ (
(
≈≈( )
)
≈≈) *
:
∆∆
 
this
∆∆ 
(
∆∆ "
ImplicitUseKindFlags
∆∆ %
.
∆∆% &
Default
∆∆& -
,
∆∆- .$
ImplicitUseTargetFlags
∆∆/ E
.
∆∆E F
Default
∆∆F M
)
∆∆M N
{
∆∆O P
}
∆∆Q R
public
»» '
MeansImplicitUseAttribute
»» (
(
»»( )"
ImplicitUseKindFlags
»») =
useKindFlags
»»> J
)
»»J K
:
……
 
this
…… 
(
…… 
useKindFlags
…… 
,
…… $
ImplicitUseTargetFlags
…… 5
.
……5 6
Default
……6 =
)
……= >
{
……? @
}
……A B
public
ÀÀ '
MeansImplicitUseAttribute
ÀÀ (
(
ÀÀ( )$
ImplicitUseTargetFlags
ÀÀ) ?
targetFlags
ÀÀ@ K
)
ÀÀK L
:
ÃÃ
 
this
ÃÃ 
(
ÃÃ "
ImplicitUseKindFlags
ÃÃ %
.
ÃÃ% &
Default
ÃÃ& -
,
ÃÃ- .
targetFlags
ÃÃ/ :
)
ÃÃ: ;
{
ÃÃ< =
}
ÃÃ> ?
public
ŒŒ '
MeansImplicitUseAttribute
ŒŒ (
(
ŒŒ( )"
ImplicitUseKindFlags
ŒŒ) =
useKindFlags
ŒŒ> J
,
ŒŒJ K$
ImplicitUseTargetFlags
ŒŒL b
targetFlags
ŒŒc n
)
ŒŒn o
{
œœ 	
UseKindFlags
–– 
=
–– 
useKindFlags
–– '
;
––' (
TargetFlags
—— 
=
—— 
targetFlags
—— %
;
——% &
}
““ 	
[
‘‘ 	
UsedImplicitly
‘‘	 
]
‘‘ 
public
‘‘ "
ImplicitUseKindFlags
‘‘  4
UseKindFlags
‘‘5 A
{
‘‘B C
get
‘‘D G
;
‘‘G H
private
‘‘I P
set
‘‘Q T
;
‘‘T U
}
‘‘V W
[
’’ 	
UsedImplicitly
’’	 
]
’’ 
public
’’ $
ImplicitUseTargetFlags
’’  6
TargetFlags
’’7 B
{
’’C D
get
’’E H
;
’’H I
private
’’J Q
set
’’R U
;
’’U V
}
’’W X
}
÷÷ 
[
ÿÿ 
Flags
ÿÿ 

]
ÿÿ
 
public
ŸŸ 

enum
ŸŸ "
ImplicitUseKindFlags
ŸŸ $
{
⁄⁄ 
Default
€€ 
=
€€ 
Access
€€ 
|
€€ 
Assign
€€ !
|
€€" #7
)InstantiatedWithFixedConstructorSignature
€€$ M
,
€€M N
Access
›› 
=
›› 
$num
›› 
,
›› 
Assign
ﬂﬂ 
=
ﬂﬂ 
$num
ﬂﬂ 
,
ﬂﬂ 7
)InstantiatedWithFixedConstructorSignature
‰‰ 1
=
‰‰2 3
$num
‰‰4 5
,
‰‰5 65
'InstantiatedNoFixedConstructorSignature
ÊÊ /
=
ÊÊ0 1
$num
ÊÊ2 3
,
ÊÊ3 4
}
ÁÁ 
[
ÌÌ 
Flags
ÌÌ 

]
ÌÌ
 
public
ÓÓ 

enum
ÓÓ $
ImplicitUseTargetFlags
ÓÓ &
{
ÔÔ 
Default
 
=
 
Itself
 
,
 
Itself
ÒÒ 
=
ÒÒ 
$num
ÒÒ 
,
ÒÒ 
Members
ÛÛ 
=
ÛÛ 
$num
ÛÛ 
,
ÛÛ 
WithMembers
ıı 
=
ıı 
Itself
ıı 
|
ıı 
Members
ıı &
}
ˆˆ 
[
¸¸ 
MeansImplicitUse
¸¸ 
(
¸¸ $
ImplicitUseTargetFlags
¸¸ ,
.
¸¸, -
WithMembers
¸¸- 8
)
¸¸8 9
]
¸¸9 :
public
˝˝ 

sealed
˝˝ 
class
˝˝  
PublicAPIAttribute
˝˝ *
:
˝˝+ ,
	Attribute
˝˝- 6
{
˛˛ 
public
ˇˇ  
PublicAPIAttribute
ˇˇ !
(
ˇˇ! "
)
ˇˇ" #
{
ˇˇ$ %
}
ˇˇ& '
public
ÄÄ  
PublicAPIAttribute
ÄÄ !
(
ÄÄ! "
[
ÄÄ" #
NotNull
ÄÄ# *
]
ÄÄ* +
string
ÄÄ, 2
comment
ÄÄ3 :
)
ÄÄ: ;
{
ÅÅ 	
Comment
ÇÇ 
=
ÇÇ 
comment
ÇÇ 
;
ÇÇ 
}
ÉÉ 	
public
ÖÖ 
string
ÖÖ 
Comment
ÖÖ 
{
ÖÖ 
get
ÖÖ  #
;
ÖÖ# $
private
ÖÖ% ,
set
ÖÖ- 0
;
ÖÖ0 1
}
ÖÖ2 3
}
ÜÜ 
[
çç 
AttributeUsage
çç 
(
çç 
AttributeTargets
çç $
.
çç$ %
	Parameter
çç% .
)
çç. /
]
çç/ 0
public
éé 

sealed
éé 
class
éé $
InstantHandleAttribute
éé .
:
éé/ 0
	Attribute
éé1 :
{
éé; <
}
éé= >
[
õõ 
AttributeUsage
õõ 
(
õõ 
AttributeTargets
õõ $
.
õõ$ %
Method
õõ% +
)
õõ+ ,
]
õõ, -
public
úú 

sealed
úú 
class
úú 
PureAttribute
úú %
:
úú& '
	Attribute
úú( 1
{
úú2 3
}
úú4 5
[
¢¢ 
AttributeUsage
¢¢ 
(
¢¢ 
AttributeTargets
¢¢ $
.
¢¢$ %
	Parameter
¢¢% .
)
¢¢. /
]
¢¢/ 0
public
££ 

sealed
££ 
class
££ $
PathReferenceAttribute
££ .
:
££/ 0
	Attribute
££1 :
{
§§ 
public
•• $
PathReferenceAttribute
•• %
(
••% &
)
••& '
{
••( )
}
••* +
public
¶¶ $
PathReferenceAttribute
¶¶ %
(
¶¶% &
[
¶¶& '
PathReference
¶¶' 4
]
¶¶4 5
string
¶¶6 <
basePath
¶¶= E
)
¶¶E F
{
ßß 	
BasePath
®® 
=
®® 
basePath
®® 
;
®®  
}
©© 	
public
´´ 
string
´´ 
BasePath
´´ 
{
´´  
get
´´! $
;
´´$ %
private
´´& -
set
´´. 1
;
´´1 2
}
´´3 4
}
¨¨ 
[
≈≈ 
AttributeUsage
≈≈ 
(
≈≈ 
AttributeTargets
≈≈ $
.
≈≈$ %
Method
≈≈% +
)
≈≈+ ,
]
≈≈, -
public
∆∆ 

sealed
∆∆ 
class
∆∆ %
SourceTemplateAttribute
∆∆ /
:
∆∆0 1
	Attribute
∆∆2 ;
{
∆∆< =
}
∆∆> ?
[
‰‰ 
AttributeUsage
‰‰ 
(
‰‰ 
AttributeTargets
‰‰ $
.
‰‰$ %
	Parameter
‰‰% .
|
‰‰/ 0
AttributeTargets
‰‰1 A
.
‰‰A B
Method
‰‰B H
,
‰‰H I
AllowMultiple
‰‰J W
=
‰‰X Y
true
‰‰Z ^
)
‰‰^ _
]
‰‰_ `
public
ÂÂ 

sealed
ÂÂ 
class
ÂÂ 
MacroAttribute
ÂÂ &
:
ÂÂ' (
	Attribute
ÂÂ) 2
{
ÊÊ 
public
ÎÎ 
string
ÎÎ 

Expression
ÎÎ  
{
ÎÎ! "
get
ÎÎ# &
;
ÎÎ& '
set
ÎÎ( +
;
ÎÎ+ ,
}
ÎÎ- .
public
ıı 
int
ıı 
Editable
ıı 
{
ıı 
get
ıı !
;
ıı! "
set
ıı# &
;
ıı& '
}
ıı( )
public
˚˚ 
string
˚˚ 
Target
˚˚ 
{
˚˚ 
get
˚˚ "
;
˚˚" #
set
˚˚$ '
;
˚˚' (
}
˚˚) *
}
¸¸ 
[
˛˛ 
AttributeUsage
˛˛ 
(
˛˛ 
AttributeTargets
˛˛ $
.
˛˛$ %
Assembly
˛˛% -
,
˛˛- .
AllowMultiple
˛˛/ <
=
˛˛= >
true
˛˛? C
)
˛˛C D
]
˛˛D E
public
ˇˇ 

sealed
ˇˇ 
class
ˇˇ 5
'AspMvcAreaMasterLocationFormatAttribute
ˇˇ ?
:
ˇˇ@ A
	Attribute
ˇˇB K
{
ÄÄ 
public
ÅÅ 5
'AspMvcAreaMasterLocationFormatAttribute
ÅÅ 6
(
ÅÅ6 7
string
ÅÅ7 =
format
ÅÅ> D
)
ÅÅD E
{
ÇÇ 	
Format
ÉÉ 
=
ÉÉ 
format
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
public
ÜÜ 
string
ÜÜ 
Format
ÜÜ 
{
ÜÜ 
get
ÜÜ "
;
ÜÜ" #
private
ÜÜ$ +
set
ÜÜ, /
;
ÜÜ/ 0
}
ÜÜ1 2
}
áá 
[
ââ 
AttributeUsage
ââ 
(
ââ 
AttributeTargets
ââ $
.
ââ$ %
Assembly
ââ% -
,
ââ- .
AllowMultiple
ââ/ <
=
ââ= >
true
ââ? C
)
ââC D
]
ââD E
public
ää 

sealed
ää 
class
ää :
,AspMvcAreaPartialViewLocationFormatAttribute
ää D
:
ääE F
	Attribute
ääG P
{
ãã 
public
åå :
,AspMvcAreaPartialViewLocationFormatAttribute
åå ;
(
åå; <
string
åå< B
format
ååC I
)
ååI J
{
çç 	
Format
éé 
=
éé 
format
éé 
;
éé 
}
èè 	
public
ëë 
string
ëë 
Format
ëë 
{
ëë 
get
ëë "
;
ëë" #
private
ëë$ +
set
ëë, /
;
ëë/ 0
}
ëë1 2
}
íí 
[
îî 
AttributeUsage
îî 
(
îî 
AttributeTargets
îî $
.
îî$ %
Assembly
îî% -
,
îî- .
AllowMultiple
îî/ <
=
îî= >
true
îî? C
)
îîC D
]
îîD E
public
ïï 

sealed
ïï 
class
ïï 3
%AspMvcAreaViewLocationFormatAttribute
ïï =
:
ïï> ?
	Attribute
ïï@ I
{
ññ 
public
óó 3
%AspMvcAreaViewLocationFormatAttribute
óó 4
(
óó4 5
string
óó5 ;
format
óó< B
)
óóB C
{
òò 	
Format
ôô 
=
ôô 
format
ôô 
;
ôô 
}
öö 	
public
úú 
string
úú 
Format
úú 
{
úú 
get
úú "
;
úú" #
private
úú$ +
set
úú, /
;
úú/ 0
}
úú1 2
}
ùù 
[
üü 
AttributeUsage
üü 
(
üü 
AttributeTargets
üü $
.
üü$ %
Assembly
üü% -
,
üü- .
AllowMultiple
üü/ <
=
üü= >
true
üü? C
)
üüC D
]
üüD E
public
†† 

sealed
†† 
class
†† 1
#AspMvcMasterLocationFormatAttribute
†† ;
:
††< =
	Attribute
††> G
{
°° 
public
¢¢ 1
#AspMvcMasterLocationFormatAttribute
¢¢ 2
(
¢¢2 3
string
¢¢3 9
format
¢¢: @
)
¢¢@ A
{
££ 	
Format
§§ 
=
§§ 
format
§§ 
;
§§ 
}
•• 	
public
ßß 
string
ßß 
Format
ßß 
{
ßß 
get
ßß "
;
ßß" #
private
ßß$ +
set
ßß, /
;
ßß/ 0
}
ßß1 2
}
®® 
[
™™ 
AttributeUsage
™™ 
(
™™ 
AttributeTargets
™™ $
.
™™$ %
Assembly
™™% -
,
™™- .
AllowMultiple
™™/ <
=
™™= >
true
™™? C
)
™™C D
]
™™D E
public
´´ 

sealed
´´ 
class
´´ 6
(AspMvcPartialViewLocationFormatAttribute
´´ @
:
´´A B
	Attribute
´´C L
{
¨¨ 
public
≠≠ 6
(AspMvcPartialViewLocationFormatAttribute
≠≠ 7
(
≠≠7 8
string
≠≠8 >
format
≠≠? E
)
≠≠E F
{
ÆÆ 	
Format
ØØ 
=
ØØ 
format
ØØ 
;
ØØ 
}
∞∞ 	
public
≤≤ 
string
≤≤ 
Format
≤≤ 
{
≤≤ 
get
≤≤ "
;
≤≤" #
private
≤≤$ +
set
≤≤, /
;
≤≤/ 0
}
≤≤1 2
}
≥≥ 
[
µµ 
AttributeUsage
µµ 
(
µµ 
AttributeTargets
µµ $
.
µµ$ %
Assembly
µµ% -
,
µµ- .
AllowMultiple
µµ/ <
=
µµ= >
true
µµ? C
)
µµC D
]
µµD E
public
∂∂ 

sealed
∂∂ 
class
∂∂ /
!AspMvcViewLocationFormatAttribute
∂∂ 9
:
∂∂: ;
	Attribute
∂∂< E
{
∑∑ 
public
∏∏ /
!AspMvcViewLocationFormatAttribute
∏∏ 0
(
∏∏0 1
string
∏∏1 7
format
∏∏8 >
)
∏∏> ?
{
ππ 	
Format
∫∫ 
=
∫∫ 
format
∫∫ 
;
∫∫ 
}
ªª 	
public
ΩΩ 
string
ΩΩ 
Format
ΩΩ 
{
ΩΩ 
get
ΩΩ "
;
ΩΩ" #
private
ΩΩ$ +
set
ΩΩ, /
;
ΩΩ/ 0
}
ΩΩ1 2
}
ææ 
[
∆∆ 
AttributeUsage
∆∆ 
(
∆∆ 
AttributeTargets
∆∆ $
.
∆∆$ %
	Parameter
∆∆% .
|
∆∆/ 0
AttributeTargets
∆∆1 A
.
∆∆A B
Method
∆∆B H
)
∆∆H I
]
∆∆I J
public
«« 

sealed
«« 
class
«« #
AspMvcActionAttribute
«« -
:
««. /
	Attribute
««0 9
{
»» 
public
…… #
AspMvcActionAttribute
…… $
(
……$ %
)
……% &
{
……' (
}
……) *
public
   #
AspMvcActionAttribute
   $
(
  $ %
string
  % +
anonymousProperty
  , =
)
  = >
{
ÀÀ 	
AnonymousProperty
ÃÃ 
=
ÃÃ 
anonymousProperty
ÃÃ  1
;
ÃÃ1 2
}
ÕÕ 	
public
œœ 
string
œœ 
AnonymousProperty
œœ '
{
œœ( )
get
œœ* -
;
œœ- .
private
œœ/ 6
set
œœ7 :
;
œœ: ;
}
œœ< =
}
–– 
[
◊◊ 
AttributeUsage
◊◊ 
(
◊◊ 
AttributeTargets
◊◊ $
.
◊◊$ %
	Parameter
◊◊% .
)
◊◊. /
]
◊◊/ 0
public
ÿÿ 

sealed
ÿÿ 
class
ÿÿ !
AspMvcAreaAttribute
ÿÿ +
:
ÿÿ, -
	Attribute
ÿÿ. 7
{
ŸŸ 
public
⁄⁄ !
AspMvcAreaAttribute
⁄⁄ "
(
⁄⁄" #
)
⁄⁄# $
{
⁄⁄% &
}
⁄⁄' (
public
€€ !
AspMvcAreaAttribute
€€ "
(
€€" #
string
€€# )
anonymousProperty
€€* ;
)
€€; <
{
‹‹ 	
AnonymousProperty
›› 
=
›› 
anonymousProperty
››  1
;
››1 2
}
ﬁﬁ 	
public
‡‡ 
string
‡‡ 
AnonymousProperty
‡‡ '
{
‡‡( )
get
‡‡* -
;
‡‡- .
private
‡‡/ 6
set
‡‡7 :
;
‡‡: ;
}
‡‡< =
}
·· 
[
ÈÈ 
AttributeUsage
ÈÈ 
(
ÈÈ 
AttributeTargets
ÈÈ $
.
ÈÈ$ %
	Parameter
ÈÈ% .
|
ÈÈ/ 0
AttributeTargets
ÈÈ1 A
.
ÈÈA B
Method
ÈÈB H
)
ÈÈH I
]
ÈÈI J
public
ÍÍ 

sealed
ÍÍ 
class
ÍÍ '
AspMvcControllerAttribute
ÍÍ 1
:
ÍÍ2 3
	Attribute
ÍÍ4 =
{
ÎÎ 
public
ÏÏ '
AspMvcControllerAttribute
ÏÏ (
(
ÏÏ( )
)
ÏÏ) *
{
ÏÏ+ ,
}
ÏÏ- .
public
ÌÌ '
AspMvcControllerAttribute
ÌÌ (
(
ÌÌ( )
string
ÌÌ) /
anonymousProperty
ÌÌ0 A
)
ÌÌA B
{
ÓÓ 	
AnonymousProperty
ÔÔ 
=
ÔÔ 
anonymousProperty
ÔÔ  1
;
ÔÔ1 2
}
 	
public
ÚÚ 
string
ÚÚ 
AnonymousProperty
ÚÚ '
{
ÚÚ( )
get
ÚÚ* -
;
ÚÚ- .
private
ÚÚ/ 6
set
ÚÚ7 :
;
ÚÚ: ;
}
ÚÚ< =
}
ÛÛ 
[
˘˘ 
AttributeUsage
˘˘ 
(
˘˘ 
AttributeTargets
˘˘ $
.
˘˘$ %
	Parameter
˘˘% .
)
˘˘. /
]
˘˘/ 0
public
˙˙ 

sealed
˙˙ 
class
˙˙ #
AspMvcMasterAttribute
˙˙ -
:
˙˙. /
	Attribute
˙˙0 9
{
˙˙: ;
}
˙˙< =
[
ÄÄ 
AttributeUsage
ÄÄ 
(
ÄÄ 
AttributeTargets
ÄÄ $
.
ÄÄ$ %
	Parameter
ÄÄ% .
)
ÄÄ. /
]
ÄÄ/ 0
public
ÅÅ 

sealed
ÅÅ 
class
ÅÅ &
AspMvcModelTypeAttribute
ÅÅ 0
:
ÅÅ1 2
	Attribute
ÅÅ3 <
{
ÅÅ= >
}
ÅÅ? @
[
ââ 
AttributeUsage
ââ 
(
ââ 
AttributeTargets
ââ $
.
ââ$ %
	Parameter
ââ% .
|
ââ/ 0
AttributeTargets
ââ1 A
.
ââA B
Method
ââB H
)
ââH I
]
ââI J
public
ää 

sealed
ää 
class
ää (
AspMvcPartialViewAttribute
ää 2
:
ää3 4
	Attribute
ää5 >
{
ää? @
}
ääA B
[
èè 
AttributeUsage
èè 
(
èè 
AttributeTargets
èè $
.
èè$ %
Class
èè% *
|
èè+ ,
AttributeTargets
èè- =
.
èè= >
Method
èè> D
)
èèD E
]
èèE F
public
êê 

sealed
êê 
class
êê -
AspMvcSupressViewErrorAttribute
êê 7
:
êê8 9
	Attribute
êê: C
{
êêD E
}
êêF G
[
óó 
AttributeUsage
óó 
(
óó 
AttributeTargets
óó $
.
óó$ %
	Parameter
óó% .
)
óó. /
]
óó/ 0
public
òò 

sealed
òò 
class
òò ,
AspMvcDisplayTemplateAttribute
òò 6
:
òò7 8
	Attribute
òò9 B
{
òòC D
}
òòE F
[
üü 
AttributeUsage
üü 
(
üü 
AttributeTargets
üü $
.
üü$ %
	Parameter
üü% .
)
üü. /
]
üü/ 0
public
†† 

sealed
†† 
class
†† +
AspMvcEditorTemplateAttribute
†† 5
:
††6 7
	Attribute
††8 A
{
††B C
}
††D E
[
ßß 
AttributeUsage
ßß 
(
ßß 
AttributeTargets
ßß $
.
ßß$ %
	Parameter
ßß% .
)
ßß. /
]
ßß/ 0
public
®® 

sealed
®® 
class
®® %
AspMvcTemplateAttribute
®® /
:
®®0 1
	Attribute
®®2 ;
{
®®< =
}
®®> ?
[
∞∞ 
AttributeUsage
∞∞ 
(
∞∞ 
AttributeTargets
∞∞ $
.
∞∞$ %
	Parameter
∞∞% .
|
∞∞/ 0
AttributeTargets
∞∞1 A
.
∞∞A B
Method
∞∞B H
)
∞∞H I
]
∞∞I J
public
±± 

sealed
±± 
class
±± !
AspMvcViewAttribute
±± +
:
±±, -
	Attribute
±±. 7
{
±±8 9
}
±±: ;
[
ææ 
AttributeUsage
ææ 
(
ææ 
AttributeTargets
ææ $
.
ææ$ %
	Parameter
ææ% .
|
ææ/ 0
AttributeTargets
ææ1 A
.
ææA B
Property
ææB J
)
ææJ K
]
ææK L
public
øø 

sealed
øø 
class
øø +
AspMvcActionSelectorAttribute
øø 5
:
øø6 7
	Attribute
øø8 A
{
øøB C
}
øøD E
[
¡¡ 
AttributeUsage
¡¡ 
(
¡¡ 
AttributeTargets
¡¡ $
.
¡¡$ %
	Parameter
¡¡% .
|
¡¡/ 0
AttributeTargets
¡¡1 A
.
¡¡A B
Property
¡¡B J
|
¡¡K L
AttributeTargets
¡¡M ]
.
¡¡] ^
Field
¡¡^ c
)
¡¡c d
]
¡¡d e
public
¬¬ 

sealed
¬¬ 
class
¬¬ ,
HtmlElementAttributesAttribute
¬¬ 6
:
¬¬7 8
	Attribute
¬¬9 B
{
√√ 
public
ƒƒ ,
HtmlElementAttributesAttribute
ƒƒ -
(
ƒƒ- .
)
ƒƒ. /
{
ƒƒ0 1
}
ƒƒ2 3
public
≈≈ ,
HtmlElementAttributesAttribute
≈≈ -
(
≈≈- .
string
≈≈. 4
name
≈≈5 9
)
≈≈9 :
{
∆∆ 	
Name
«« 
=
«« 
name
«« 
;
«« 
}
»» 	
public
   
string
   
Name
   
{
   
get
    
;
    !
private
  " )
set
  * -
;
  - .
}
  / 0
}
ÀÀ 
[
ÕÕ 
AttributeUsage
ÕÕ 
(
ÕÕ 
AttributeTargets
ÕÕ $
.
ÕÕ$ %
	Parameter
ÕÕ% .
|
ÕÕ/ 0
AttributeTargets
ÕÕ1 A
.
ÕÕA B
Field
ÕÕB G
|
ÕÕH I
AttributeTargets
ÕÕJ Z
.
ÕÕZ [
Property
ÕÕ[ c
)
ÕÕc d
]
ÕÕd e
public
ŒŒ 

sealed
ŒŒ 
class
ŒŒ )
HtmlAttributeValueAttribute
ŒŒ 3
:
ŒŒ4 5
	Attribute
ŒŒ6 ?
{
œœ 
public
–– )
HtmlAttributeValueAttribute
–– *
(
––* +
[
––+ ,
NotNull
––, 3
]
––3 4
string
––5 ;
name
––< @
)
––@ A
{
—— 	
Name
““ 
=
““ 
name
““ 
;
““ 
}
”” 	
[
’’ 	
NotNull
’’	 
]
’’ 
public
’’ 
string
’’ 
Name
’’  $
{
’’% &
get
’’' *
;
’’* +
private
’’, 3
set
’’4 7
;
’’7 8
}
’’9 :
}
÷÷ 
[
›› 
AttributeUsage
›› 
(
›› 
AttributeTargets
›› $
.
››$ %
	Parameter
››% .
|
››/ 0
AttributeTargets
››1 A
.
››A B
Method
››B H
)
››H I
]
››I J
public
ﬁﬁ 

sealed
ﬁﬁ 
class
ﬁﬁ #
RazorSectionAttribute
ﬁﬁ -
:
ﬁﬁ. /
	Attribute
ﬁﬁ0 9
{
ﬁﬁ: ;
}
ﬁﬁ< =
[
‰‰ 
AttributeUsage
‰‰ 
(
‰‰ 
AttributeTargets
‰‰ $
.
‰‰$ %
Method
‰‰% +
|
‰‰, -
AttributeTargets
‰‰. >
.
‰‰> ?
Constructor
‰‰? J
|
‰‰K L
AttributeTargets
‰‰M ]
.
‰‰] ^
Property
‰‰^ f
)
‰‰f g
]
‰‰g h
public
ÂÂ 

sealed
ÂÂ 
class
ÂÂ '
CollectionAccessAttribute
ÂÂ 1
:
ÂÂ2 3
	Attribute
ÂÂ4 =
{
ÊÊ 
public
ÁÁ '
CollectionAccessAttribute
ÁÁ (
(
ÁÁ( )"
CollectionAccessType
ÁÁ) ="
collectionAccessType
ÁÁ> R
)
ÁÁR S
{
ËË 	"
CollectionAccessType
ÈÈ  
=
ÈÈ! ""
collectionAccessType
ÈÈ# 7
;
ÈÈ7 8
}
ÍÍ 	
public
ÏÏ "
CollectionAccessType
ÏÏ #"
CollectionAccessType
ÏÏ$ 8
{
ÏÏ9 :
get
ÏÏ; >
;
ÏÏ> ?
private
ÏÏ@ G
set
ÏÏH K
;
ÏÏK L
}
ÏÏM N
}
ÌÌ 
[
ÔÔ 
Flags
ÔÔ 

]
ÔÔ
 
public
 

enum
 "
CollectionAccessType
 $
{
ÒÒ 
None
ÛÛ 
=
ÛÛ 
$num
ÛÛ 
,
ÛÛ 
Read
ıı 
=
ıı 
$num
ıı 
,
ıı #
ModifyExistingContent
˜˜ 
=
˜˜ 
$num
˜˜  !
,
˜˜! "
UpdatedContent
˘˘ 
=
˘˘ #
ModifyExistingContent
˘˘ .
|
˘˘/ 0
$num
˘˘1 2
}
˙˙ 
[
ÅÅ 
AttributeUsage
ÅÅ 
(
ÅÅ 
AttributeTargets
ÅÅ $
.
ÅÅ$ %
Method
ÅÅ% +
)
ÅÅ+ ,
]
ÅÅ, -
public
ÇÇ 

sealed
ÇÇ 
class
ÇÇ &
AssertionMethodAttribute
ÇÇ 0
:
ÇÇ1 2
	Attribute
ÇÇ3 <
{
ÇÇ= >
}
ÇÇ? @
[
ââ 
AttributeUsage
ââ 
(
ââ 
AttributeTargets
ââ $
.
ââ$ %
	Parameter
ââ% .
)
ââ. /
]
ââ/ 0
public
ää 

sealed
ää 
class
ää )
AssertionConditionAttribute
ää 3
:
ää4 5
	Attribute
ää6 ?
{
ãã 
public
åå )
AssertionConditionAttribute
åå *
(
åå* +$
AssertionConditionType
åå+ A
conditionType
ååB O
)
ååO P
{
çç 	
ConditionType
éé 
=
éé 
conditionType
éé )
;
éé) *
}
èè 	
public
ëë $
AssertionConditionType
ëë %
ConditionType
ëë& 3
{
ëë4 5
get
ëë6 9
;
ëë9 :
private
ëë; B
set
ëëC F
;
ëëF G
}
ëëH I
}
íí 
public
òò 

enum
òò $
AssertionConditionType
òò &
{
ôô 
IS_TRUE
õõ 
=
õõ 
$num
õõ 
,
õõ 
IS_FALSE
ùù 
=
ùù 
$num
ùù 
,
ùù 
IS_NULL
üü 
=
üü 
$num
üü 
,
üü 
IS_NOT_NULL
°° 
=
°° 
$num
°° 
,
°° 
}
¢¢ 
[
®® 
Obsolete
®® 
(
®® 
$str
®® ;
)
®®; <
]
®®< =
[
©© 
AttributeUsage
©© 
(
©© 
AttributeTargets
©© $
.
©©$ %
Method
©©% +
)
©©+ ,
]
©©, -
public
™™ 

sealed
™™ 
class
™™ (
TerminatesProgramAttribute
™™ 2
:
™™3 4
	Attribute
™™5 >
{
™™? @
}
™™A B
[
±± 
AttributeUsage
±± 
(
±± 
AttributeTargets
±± $
.
±±$ %
Method
±±% +
)
±±+ ,
]
±±, -
public
≤≤ 

sealed
≤≤ 
class
≤≤ !
LinqTunnelAttribute
≤≤ +
:
≤≤, -
	Attribute
≤≤. 7
{
≤≤8 9
}
≤≤: ;
[
∑∑ 
AttributeUsage
∑∑ 
(
∑∑ 
AttributeTargets
∑∑ $
.
∑∑$ %
	Parameter
∑∑% .
)
∑∑. /
]
∑∑/ 0
public
∏∏ 

sealed
∏∏ 
class
∏∏ $
NoEnumerationAttribute
∏∏ .
:
∏∏/ 0
	Attribute
∏∏1 :
{
∏∏; <
}
∏∏= >
[
ΩΩ 
AttributeUsage
ΩΩ 
(
ΩΩ 
AttributeTargets
ΩΩ $
.
ΩΩ$ %
	Parameter
ΩΩ% .
)
ΩΩ. /
]
ΩΩ/ 0
public
ææ 

sealed
ææ 
class
ææ #
RegexPatternAttribute
ææ -
:
ææ. /
	Attribute
ææ0 9
{
ææ: ;
}
ææ< =
[
ƒƒ 
AttributeUsage
ƒƒ 
(
ƒƒ 
AttributeTargets
ƒƒ $
.
ƒƒ$ %
Class
ƒƒ% *
)
ƒƒ* +
]
ƒƒ+ ,
public
≈≈ 

sealed
≈≈ 
class
≈≈ '
XamlItemsControlAttribute
≈≈ 1
:
≈≈2 3
	Attribute
≈≈4 =
{
≈≈> ?
}
≈≈@ A
[
–– 
AttributeUsage
–– 
(
–– 
AttributeTargets
–– $
.
––$ %
Property
––% -
)
––- .
]
––. /
public
—— 

sealed
—— 
class
—— 4
&XamlItemBindingOfItemsControlAttribute
—— >
:
——? @
	Attribute
——A J
{
——K L
}
——M N
[
”” 
AttributeUsage
”” 
(
”” 
AttributeTargets
”” $
.
””$ %
Class
””% *
,
””* +
AllowMultiple
””, 9
=
””: ;
true
””< @
)
””@ A
]
””A B
public
‘‘ 

sealed
‘‘ 
class
‘‘ *
AspChildControlTypeAttribute
‘‘ 4
:
‘‘5 6
	Attribute
‘‘7 @
{
’’ 
public
÷÷ *
AspChildControlTypeAttribute
÷÷ +
(
÷÷+ ,
string
÷÷, 2
tagName
÷÷3 :
,
÷÷: ;
Type
÷÷< @
controlType
÷÷A L
)
÷÷L M
{
◊◊ 	
TagName
ÿÿ 
=
ÿÿ 
tagName
ÿÿ 
;
ÿÿ 
ControlType
ŸŸ 
=
ŸŸ 
controlType
ŸŸ %
;
ŸŸ% &
}
⁄⁄ 	
public
‹‹ 
string
‹‹ 
TagName
‹‹ 
{
‹‹ 
get
‹‹  #
;
‹‹# $
private
‹‹% ,
set
‹‹- 0
;
‹‹0 1
}
‹‹2 3
public
›› 
Type
›› 
ControlType
›› 
{
››  !
get
››" %
;
››% &
private
››' .
set
››/ 2
;
››2 3
}
››4 5
}
ﬁﬁ 
[
‡‡ 
AttributeUsage
‡‡ 
(
‡‡ 
AttributeTargets
‡‡ $
.
‡‡$ %
Property
‡‡% -
|
‡‡. /
AttributeTargets
‡‡0 @
.
‡‡@ A
Method
‡‡A G
)
‡‡G H
]
‡‡H I
public
·· 

sealed
·· 
class
·· #
AspDataFieldAttribute
·· -
:
··. /
	Attribute
··0 9
{
··: ;
}
··< =
[
„„ 
AttributeUsage
„„ 
(
„„ 
AttributeTargets
„„ $
.
„„$ %
Property
„„% -
|
„„. /
AttributeTargets
„„0 @
.
„„@ A
Method
„„A G
)
„„G H
]
„„H I
public
‰‰ 

sealed
‰‰ 
class
‰‰ $
AspDataFieldsAttribute
‰‰ .
:
‰‰/ 0
	Attribute
‰‰1 :
{
‰‰; <
}
‰‰= >
[
ÊÊ 
AttributeUsage
ÊÊ 
(
ÊÊ 
AttributeTargets
ÊÊ $
.
ÊÊ$ %
Property
ÊÊ% -
)
ÊÊ- .
]
ÊÊ. /
public
ÁÁ 

sealed
ÁÁ 
class
ÁÁ (
AspMethodPropertyAttribute
ÁÁ 2
:
ÁÁ3 4
	Attribute
ÁÁ5 >
{
ÁÁ? @
}
ÁÁA B
[
ÈÈ 
AttributeUsage
ÈÈ 
(
ÈÈ 
AttributeTargets
ÈÈ $
.
ÈÈ$ %
Class
ÈÈ% *
,
ÈÈ* +
AllowMultiple
ÈÈ, 9
=
ÈÈ: ;
true
ÈÈ< @
)
ÈÈ@ A
]
ÈÈA B
public
ÍÍ 

sealed
ÍÍ 
class
ÍÍ +
AspRequiredAttributeAttribute
ÍÍ 5
:
ÍÍ6 7
	Attribute
ÍÍ8 A
{
ÎÎ 
public
ÏÏ +
AspRequiredAttributeAttribute
ÏÏ ,
(
ÏÏ, -
[
ÏÏ- .
NotNull
ÏÏ. 5
]
ÏÏ5 6
string
ÏÏ7 =
	attribute
ÏÏ> G
)
ÏÏG H
{
ÌÌ 	
	Attribute
ÓÓ 
=
ÓÓ 
	attribute
ÓÓ !
;
ÓÓ! "
}
ÔÔ 	
public
ÒÒ 
string
ÒÒ 
	Attribute
ÒÒ 
{
ÒÒ  !
get
ÒÒ" %
;
ÒÒ% &
private
ÒÒ' .
set
ÒÒ/ 2
;
ÒÒ2 3
}
ÒÒ4 5
}
ÚÚ 
[
ÙÙ 
AttributeUsage
ÙÙ 
(
ÙÙ 
AttributeTargets
ÙÙ $
.
ÙÙ$ %
Property
ÙÙ% -
)
ÙÙ- .
]
ÙÙ. /
public
ıı 

sealed
ıı 
class
ıı &
AspTypePropertyAttribute
ıı 0
:
ıı1 2
	Attribute
ıı3 <
{
ˆˆ 
public
˜˜ 
bool
˜˜ )
CreateConstructorReferences
˜˜ /
{
˜˜0 1
get
˜˜2 5
;
˜˜5 6
private
˜˜7 >
set
˜˜? B
;
˜˜B C
}
˜˜D E
public
˘˘ &
AspTypePropertyAttribute
˘˘ '
(
˘˘' (
bool
˘˘( ,)
createConstructorReferences
˘˘- H
)
˘˘H I
{
˙˙ 	)
CreateConstructorReferences
˚˚ '
=
˚˚( ))
createConstructorReferences
˚˚* E
;
˚˚E F
}
¸¸ 	
}
˝˝ 
[
ˇˇ 
AttributeUsage
ˇˇ 
(
ˇˇ 
AttributeTargets
ˇˇ $
.
ˇˇ$ %
Assembly
ˇˇ% -
,
ˇˇ- .
AllowMultiple
ˇˇ/ <
=
ˇˇ= >
true
ˇˇ? C
)
ˇˇC D
]
ˇˇD E
public
ÄÄ 

sealed
ÄÄ 
class
ÄÄ +
RazorImportNamespaceAttribute
ÄÄ 5
:
ÄÄ6 7
	Attribute
ÄÄ8 A
{
ÅÅ 
public
ÇÇ +
RazorImportNamespaceAttribute
ÇÇ ,
(
ÇÇ, -
string
ÇÇ- 3
name
ÇÇ4 8
)
ÇÇ8 9
{
ÉÉ 	
Name
ÑÑ 
=
ÑÑ 
name
ÑÑ 
;
ÑÑ 
}
ÖÖ 	
public
áá 
string
áá 
Name
áá 
{
áá 
get
áá  
;
áá  !
private
áá" )
set
áá* -
;
áá- .
}
áá/ 0
}
àà 
[
ää 
AttributeUsage
ää 
(
ää 
AttributeTargets
ää $
.
ää$ %
Assembly
ää% -
,
ää- .
AllowMultiple
ää/ <
=
ää= >
true
ää? C
)
ääC D
]
ääD E
public
ãã 

sealed
ãã 
class
ãã %
RazorInjectionAttribute
ãã /
:
ãã0 1
	Attribute
ãã2 ;
{
åå 
public
çç %
RazorInjectionAttribute
çç &
(
çç& '
string
çç' -
type
çç. 2
,
çç2 3
string
çç4 :
	fieldName
çç; D
)
ççD E
{
éé 	
Type
èè 
=
èè 
type
èè 
;
èè 
	FieldName
êê 
=
êê 
	fieldName
êê !
;
êê! "
}
ëë 	
public
ìì 
string
ìì 
Type
ìì 
{
ìì 
get
ìì  
;
ìì  !
private
ìì" )
set
ìì* -
;
ìì- .
}
ìì/ 0
public
îî 
string
îî 
	FieldName
îî 
{
îî  !
get
îî" %
;
îî% &
private
îî' .
set
îî/ 2
;
îî2 3
}
îî4 5
}
ïï 
[
óó 
AttributeUsage
óó 
(
óó 
AttributeTargets
óó $
.
óó$ %
Method
óó% +
)
óó+ ,
]
óó, -
public
òò 

sealed
òò 
class
òò (
RazorHelperCommonAttribute
òò 2
:
òò3 4
	Attribute
òò5 >
{
òò? @
}
òòA B
[
öö 
AttributeUsage
öö 
(
öö 
AttributeTargets
öö $
.
öö$ %
Property
öö% -
)
öö- .
]
öö. /
public
õõ 

sealed
õõ 
class
õõ "
RazorLayoutAttribute
õõ ,
:
õõ- .
	Attribute
õõ/ 8
{
õõ9 :
}
õõ; <
[
ùù 
AttributeUsage
ùù 
(
ùù 
AttributeTargets
ùù $
.
ùù$ %
Method
ùù% +
)
ùù+ ,
]
ùù, -
public
ûû 

sealed
ûû 
class
ûû .
 RazorWriteLiteralMethodAttribute
ûû 8
:
ûû9 :
	Attribute
ûû; D
{
ûûE F
}
ûûG H
[
†† 
AttributeUsage
†† 
(
†† 
AttributeTargets
†† $
.
††$ %
Method
††% +
)
††+ ,
]
††, -
public
°° 

sealed
°° 
class
°° '
RazorWriteMethodAttribute
°° 1
:
°°2 3
	Attribute
°°4 =
{
°°> ?
}
°°@ A
[
££ 
AttributeUsage
££ 
(
££ 
AttributeTargets
££ $
.
££$ %
	Parameter
££% .
)
££. /
]
££/ 0
public
§§ 

sealed
§§ 
class
§§ 0
"RazorWriteMethodParameterAttribute
§§ :
:
§§; <
	Attribute
§§= F
{
§§G H
}
§§I J
[
¨¨ 
AttributeUsage
¨¨ 
(
¨¨ 
AttributeTargets
¨¨ $
.
¨¨$ %
All
¨¨% (
)
¨¨( )
]
¨¨) *
public
≠≠ 

sealed
≠≠ 
class
≠≠ 
	NoReorder
≠≠ !
:
≠≠" #
	Attribute
≠≠$ -
{
≠≠. /
}
≠≠0 1
}ÆÆ ˛
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
]##) *¡9
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
TDestino	33z Ç
:
33É Ñ
class
33Ö ä
where
33ã ê
TOrigem
33ë ò
:
33ô ö
class
33õ †
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
Instance	77z Ç
)
77Ç É
;
77É Ñ
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
	TProperty	FFz É
>
FFÉ Ñ
>
FFÑ Ö
propertyLambda
FFÜ î
)
FFî ï
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
propertyLambda	LL} ã
)
LLã å
)
LLå ç
;
LLç é
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
propertyLambda	PP| ä
)
PPä ã
)
PPã å
;
PPå ç
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
)	RR Ä
)
RRÄ Å
throwSS 
newSS 
ArgumentExceptionSS +
(SS+ ,
stringSS, 2
.SS2 3
FormatSS3 9
(SS9 :
$str	SS: Å
,
SSÅ Ç
propertyLambda
SSÉ ë
,
SSë í
type
SSì ó
)
SSó ò
)
SSò ô
;
SSô ö
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
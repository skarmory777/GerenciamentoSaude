Ÿ%
\C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Configuracoes\MDFeConfiguracao.cs
	namespace)) 	
MDFe))
 
.)) 
Utils)) 
.)) 
Configuracoes)) "
{** 
public++ 

class++ 
MDFeConfiguracao++ !
{,, 
private-- 
static--  
MDFeVersaoWebService-- +
_versaoWebService--, =
;--= >
public// 
MDFeConfiguracao// 
(//  
)//  !
{00 	
VersaoWebService11 
=11 
new11 " 
MDFeVersaoWebService11# 7
(117 8
)118 9
;119 :
}22 	
public44 
static44 #
ConfiguracaoCertificado44 -#
ConfiguracaoCertificado44. E
{44F G
get44H K
;44K L
set44M P
;44P Q
}44R S
public66 
static66 
bool66 
IsSalvarXml66 &
{66' (
get66) ,
;66, -
set66. 1
;661 2
}663 4
public77 
static77 
string77 
CaminhoSchemas77 +
{77, -
get77. 1
;771 2
set773 6
;776 7
}778 9
public88 
static88 
string88 
CaminhoSalvarXml88 -
{88. /
get880 3
;883 4
set885 8
;888 9
}88: ;
public:: 
static::  
MDFeVersaoWebService:: *
VersaoWebService::+ ;
{;; 	
get<< 
{<< 
return<< #
GetMdfeVersaoWebService<< 0
(<<0 1
)<<1 2
;<<2 3
}<<4 5
set== 
{== 
_versaoWebService== #
===$ %
value==& +
;==+ ,
}==- .
}>> 	
private@@ 
static@@  
MDFeVersaoWebService@@ +#
GetMdfeVersaoWebService@@, C
(@@C D
)@@D E
{AA 	
ifBB 
(BB 
_versaoWebServiceBB !
==BB" $
nullBB% )
)BB) *
_versaoWebServiceCC !
=CC" #
newCC$ ' 
MDFeVersaoWebServiceCC( <
(CC< =
)CC= >
;CC> ?
returnEE 
_versaoWebServiceEE $
;EE$ %
}FF 	
publicHH 
staticHH 
X509Certificate2HH &
X509Certificate2HH' 7
{HH8 9
getHH: =
{HH> ?
returnHH@ F
ObterCertificadoHHG W
(HHW X
)HHX Y
;HHY Z
}HH[ \
}HH] ^
publicKK 
staticKK 
boolKK 
NaoSalvarXmlKK '
(KK' (
)KK( )
{LL 	
returnMM 
!MM 
IsSalvarXmlMM 
;MM  
}NN 	
privatePP 
staticPP 
X509Certificate2PP '
ObterCertificadoPP( 8
(PP8 9
)PP9 :
{QQ 	
returnRR 
CertificadoDigitalRR %
.RR% &
ObterCertificadoRR& 6
(RR6 7#
ConfiguracaoCertificadoRR7 N
)RRN O
;RRO P
}SS 	
}TT 
publicVV 

classVV  
MDFeVersaoWebServiceVV %
{WW 
publicXX 
intXX 
TimeOutXX 
{XX 
getXX  
;XX  !
setXX" %
;XX% &
}XX' (
publicYY 
	EstadoNfeYY 

UfEmitenteYY #
{YY$ %
getYY& )
;YY) *
setYY+ .
;YY. /
}YY0 1
publicZZ 
TipoAmbienteZZ 
TipoAmbienteZZ (
{ZZ) *
getZZ+ .
;ZZ. /
setZZ0 3
;ZZ3 4
}ZZ5 6
public[[ 
VersaoServico[[ 
VersaoMDFeRecepcao[[ /
{[[0 1
get[[2 5
;[[5 6
set[[7 :
;[[: ;
}[[< =
public\\ 
VersaoServico\\ !
VersaoMDFeRetRecepcao\\ 2
{\\3 4
get\\5 8
;\\8 9
set\\: =
;\\= >
}\\? @
public]] 
VersaoServico]] $
VersaoMDFeRecepcaoEvento]] 5
{]]6 7
get]]8 ;
;]]; <
set]]= @
;]]@ A
}]]B C
public^^ 
VersaoServico^^ 
VersaoMDFeConsulta^^ /
{^^0 1
get^^2 5
;^^5 6
set^^7 :
;^^: ;
}^^< =
public__ 
VersaoServico__ #
VersaoMDFeStatusServico__ 4
{__5 6
get__7 :
;__: ;
set__< ?
;__? @
}__A B
public`` 
VersaoServico``  
VersaoMDFeConsNaoEnc`` 1
{``2 3
get``4 7
;``7 8
set``9 <
;``< =
}``> ?
}aa 
}bb ‹P
OC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFe.cs
	namespace++ 	
MDFe++
 
.++ 
Utils++ 
.++ 
	Extencoes++ 
{,, 
public-- 

static-- 
class-- 
ExtMDFe-- 
{.. 
public// 
static// 
MDFEletronico// #
Valida//$ *
(//* +
this//+ /
MDFEletronico//0 =
mdfe//> B
)//B C
{00 	
if11 
(11 
mdfe11 
==11 
null11 
)11 
throw11 #
new11$ '
ArgumentException11( 9
(119 :
$str11: ^
)11^ _
;11_ `
var33 
xmlMdfe33 
=33 

FuncoesXml33 $
.33$ %
ClasseParaXmlString33% 8
(338 9
mdfe339 =
)33= >
;33> ?
	Validador55 
.55 
Valida55 
(55 
xmlMdfe55 $
,55$ %
$str55& 6
)556 7
;557 8
var77 
	tipoModal77 
=77 
mdfe77  
.77  !
InfMDFe77! (
.77( )
InfModal77) 1
.771 2
Modal772 7
.777 8
GetType778 ?
(77? @
)77@ A
;77A B
var88 
xmlModal88 
=88 

FuncoesXml88 %
.88% &
ClasseParaXmlString88& 9
(889 :
mdfe88: >
.88> ?
InfMDFe88? F
.88F G
InfModal88G O
)88O P
;88P Q
if;; 
(;; 
	tipoModal;; 
==;; 
typeof;; #
(;;# $
MDFeRodo;;$ ,
);;, -
);;- .
{<< 
	Validador== 
.== 
Valida==  
(==  !
xmlModal==! )
,==) *
$str==+ J
)==J K
;==K L
}>> 
if@@ 
(@@ 
	tipoModal@@ 
==@@ 
typeof@@ #
(@@# $
	MDFeAereo@@$ -
)@@- .
)@@. /
{AA 
	ValidadorBB 
.BB 
ValidaBB  
(BB  !
xmlModalBB! )
,BB) *
$strBB+ E
)BBE F
;BBF G
}CC 
ifEE 
(EE 
	tipoModalEE 
==EE 
typeofEE #
(EE# $
	MDFeAquavEE$ -
)EE- .
)EE. /
{FF 
	ValidadorGG 
.GG 
ValidaGG  
(GG  !
xmlModalGG! )
,GG) *
$strGG+ J
)GGJ K
;GGK L
}HH 
ifJJ 
(JJ 
	tipoModalJJ 
==JJ 
typeofJJ #
(JJ# $

MDFeFerrovJJ$ .
)JJ. /
)JJ/ 0
{KK 
	ValidadorLL 
.LL 
ValidaLL  
(LL  !
xmlModalLL! )
,LL) *
$strLL+ K
)LLK L
;LLL M
}MM 
returnOO 
mdfeOO 
;OO 
}PP 	
publicRR 
staticRR 
MDFEletronicoRR #
AssinaRR$ *
(RR* +
thisRR+ /
MDFEletronicoRR0 =
mdfeRR> B
)RRB C
{SS 	
ifTT 
(TT 
mdfeTT 
==TT 
nullTT 
)TT 
throwTT #
newTT$ '
ArgumentExceptionTT( 9
(TT9 :
$strTT: ^
)TT^ _
;TT_ `
varVV !
modeloDocumentoFiscalVV %
=VV& '
mdfeVV( ,
.VV, -
InfMDFeVV- 4
.VV4 5
IdeVV5 8
.VV8 9
ModVV9 <
;VV< =
varWW 
tipoEmissaoWW 
=WW 
(WW 
intWW "
)WW" #
mdfeWW# '
.WW' (
InfMDFeWW( /
.WW/ 0
IdeWW0 3
.WW3 4
TpEmisWW4 :
;WW: ;
varXX 
codigoNumericoXX 
=XX  
mdfeXX! %
.XX% &
InfMDFeXX& -
.XX- .
IdeXX. 1
.XX1 2
CMDFXX2 6
;XX6 7
varYY 
estadoYY 
=YY 
mdfeYY 
.YY 
InfMDFeYY %
.YY% &
IdeYY& )
.YY) *
CUFYY* -
;YY- .
varZZ 
dataEHoraEmissaoZZ  
=ZZ! "
mdfeZZ# '
.ZZ' (
InfMDFeZZ( /
.ZZ/ 0
IdeZZ0 3
.ZZ3 4
DhEmiZZ4 9
;ZZ9 :
var[[ 
cnpj[[ 
=[[ 
mdfe[[ 
.[[ 
InfMDFe[[ #
.[[# $
Emit[[$ (
.[[( )
CNPJ[[) -
;[[- .
var\\ 
numeroDocumento\\ 
=\\  !
mdfe\\" &
.\\& '
InfMDFe\\' .
.\\. /
Ide\\/ 2
.\\2 3
NMDF\\3 7
;\\7 8
int]] 
serie]] 
=]] 
mdfe]] 
.]] 
InfMDFe]] $
.]]$ %
Ide]]% (
.]]( )
Serie]]) .
;]]. /
var__ 

dadosChave__ 
=__ 
ChaveFiscal__ (
.__( )

ObterChave__) 3
(__3 4
estado__4 :
,__: ;
dataEHoraEmissao__< L
,__L M
cnpj__N R
,__R S!
modeloDocumentoFiscal__T i
,__i j
serie__k p
,__p q
numeroDocumento	__r Å
,
__Å Ç
tipoEmissao
__É é
,
__é è
codigoNumerico
__ê û
)
__û ü
;
__ü †
mdfeaa 
.aa 
InfMDFeaa 
.aa 
Idaa 
=aa 
$straa $
+aa% &

dadosChaveaa' 1
.aa1 2
Chaveaa2 7
;aa7 8
mdfebb 
.bb 
InfMDFebb 
.bb 
Versaobb 
=bb  !
VersaoServicobb" /
.bb/ 0
	Versao100bb0 9
;bb9 :
mdfecc 
.cc 
InfMDFecc 
.cc 
Idecc 
.cc 
CDVcc  
=cc! "

dadosChavecc# -
.cc- .
DigitoVerificadorcc. ?
;cc? @
varee 

assinaturaee 
=ee 
AssinaturaDigitalee .
.ee. /
Assinaee/ 5
(ee5 6
mdfeee6 :
,ee: ;
mdfeee< @
.ee@ A
InfMDFeeeA H
.eeH I
IdeeI K
,eeK L
MDFeConfiguracaoeeM ]
.ee] ^
X509Certificate2ee^ n
)een o
;eeo p
mdfegg 
.gg 
	Signaturegg 
=gg 

assinaturagg '
;gg' (
returnii 
mdfeii 
;ii 
}jj 	
publicll 
staticll 
stringll 
	XmlStringll &
(ll& '
thisll' +
MDFEletronicoll, 9
mdfell: >
)ll> ?
{mm 	
returnnn 

FuncoesXmlnn 
.nn 
ClasseParaXmlStringnn 1
(nn1 2
mdfenn2 6
)nn6 7
;nn7 8
}oo 	
publicqq 
staticqq 
voidqq 
SalvarXmlEmDiscoqq +
(qq+ ,
thisqq, 0
MDFEletronicoqq1 >
mdfeqq? C
,qqC D
stringqqE K
nomeArquivoqqL W
=qqX Y
nullqqZ ^
)qq^ _
{rr 	
ifss 
(ss 
MDFeConfiguracaoss  
.ss  !
NaoSalvarXmlss! -
(ss- .
)ss. /
)ss/ 0
returnss1 7
;ss7 8
ifuu 
(uu 
stringuu 
.uu 
IsNullOrEmptyuu $
(uu$ %
nomeArquivouu% 0
)uu0 1
)uu1 2
nomeArquivovv 
=vv 
MDFeConfiguracaovv .
.vv. /
CaminhoSalvarXmlvv/ ?
+vv@ A
$strvvB F
+vvG H
mdfevvI M
.vvM N
ChavevvN S
(vvS T
)vvT U
+vvV W
$strvvX c
;vvc d

FuncoesXmlxx 
.xx  
ClasseParaArquivoXmlxx +
(xx+ ,
mdfexx, 0
,xx0 1
nomeArquivoxx2 =
)xx= >
;xx> ?
}yy 	
public{{ 
static{{ 
string{{ 
Chave{{ "
({{" #
this{{# '
MDFEletronico{{( 5
mdfe{{6 :
){{: ;
{|| 	
var}} 
chave}} 
=}} 
mdfe}} 
.}} 
InfMDFe}} $
.}}$ %
Id}}% '
.}}' (
	Substring}}( 1
(}}1 2
$num}}2 3
,}}3 4
$num}}5 7
)}}7 8
;}}8 9
return~~ 
chave~~ 
;~~ 
} 	
public
ÅÅ 
static
ÅÅ 
string
ÅÅ 
CNPJEmitente
ÅÅ )
(
ÅÅ) *
this
ÅÅ* .
MDFEletronico
ÅÅ/ <
mdfe
ÅÅ= A
)
ÅÅA B
{
ÇÇ 	
var
ÉÉ 
cnpj
ÉÉ 
=
ÉÉ 
mdfe
ÉÉ 
.
ÉÉ 
InfMDFe
ÉÉ #
.
ÉÉ# $
Emit
ÉÉ$ (
.
ÉÉ( )
CNPJ
ÉÉ) -
;
ÉÉ- .
return
ÖÖ 
cnpj
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
public
àà 
static
àà 
	EstadoNfe
àà 

UFEmitente
àà  *
(
àà* +
this
àà+ /
MDFEletronico
àà0 =
mdfe
àà> B
)
ààB C
{
ââ 	
var
ää 
estadoUf
ää 
=
ää 
mdfe
ää 
.
ää  
InfMDFe
ää  '
.
ää' (
Emit
ää( ,
.
ää, -
	EnderEmit
ää- 6
.
ää6 7
UF
ää7 9
;
ää9 :
return
åå 
estadoUf
åå 
;
åå 
}
çç 	
public
èè 
static
èè 
long
èè )
CodigoIbgeMunicipioEmitente
èè 6
(
èè6 7
this
èè7 ;
MDFEletronico
èè< I
mdfe
èèJ N
)
èèN O
{
êê 	
var
ëë 
codigo
ëë 
=
ëë 
mdfe
ëë 
.
ëë 
InfMDFe
ëë %
.
ëë% &
Emit
ëë& *
.
ëë* +
	EnderEmit
ëë+ 4
.
ëë4 5
CMun
ëë5 9
;
ëë9 :
return
ìì 
codigo
ìì 
;
ìì 
}
îî 	
}
ïï 
}ññ ∑
[C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeConsReciMDFe.cs
	namespace'' 	
MDFe''
 
.'' 
Utils'' 
.'' 
	Extencoes'' 
{(( 
public)) 

static)) 
class)) 
ExtMDFeConsReciMDFe)) +
{** 
public++ 
static++ 
void++ 
ValidaSchema++ '
(++' (
this++( ,
MDFeConsReciMDFe++- =
consReciMDFe++> J
)++J K
{,, 	
var-- 
xmlValidacao-- 
=-- 
consReciMDFe-- +
.--+ ,
	XmlString--, 5
(--5 6
)--6 7
;--7 8
	Validador// 
.// 
Valida// 
(// 
xmlValidacao// )
,//) *
$str//+ C
)//C D
;//D E
}00 	
public22 
static22 
string22 
	XmlString22 &
(22& '
this22' +
MDFeConsReciMDFe22, <
consReciMDFe22= I
)22I J
{33 	
return44 

FuncoesXml44 
.44 
ClasseParaXmlString44 1
(441 2
consReciMDFe442 >
)44> ?
;44? @
}55 	
public77 
static77 
XmlDocument77 !
CriaRequestWs77" /
(77/ 0
this770 4
MDFeConsReciMDFe775 E
consReciMDFe77F R
)77R S
{88 	
var99 
request99 
=99 
new99 
XmlDocument99 )
(99) *
)99* +
;99+ ,
request:: 
.:: 
LoadXml:: 
(:: 
consReciMDFe:: (
.::( )
	XmlString::) 2
(::2 3
)::3 4
)::4 5
;::5 6
return<< 
request<< 
;<< 
}== 	
public?? 
static?? 
void?? 
SalvarXmlEmDisco?? +
(??+ ,
this??, 0
MDFeConsReciMDFe??1 A
consReciMDFe??B N
)??N O
{@@ 	
ifAA 
(AA 
MDFeConfiguracaoAA  
.AA  !
NaoSalvarXmlAA! -
(AA- .
)AA. /
)AA/ 0
returnAA1 7
;AA7 8
varCC 

caminhoXmlCC 
=CC 
MDFeConfiguracaoCC -
.CC- .
CaminhoSalvarXmlCC. >
;CC> ?
varEE 
arquivoSalvarEE 
=EE 

caminhoXmlEE  *
+EE+ ,
$strEE- 1
+EE2 3
consReciMDFeEE4 @
.EE@ A
NRecEEA E
+EEF G
$strEEH V
;EEV W

FuncoesXmlGG 
.GG  
ClasseParaArquivoXmlGG +
(GG+ ,
consReciMDFeGG, 8
,GG8 9
arquivoSalvarGG: G
)GGG H
;GGH I
}HH 	
}II 
}JJ £
ZC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeConsSitMDFe.cs
	namespace'' 	
MDFe''
 
.'' 
Utils'' 
.'' 
	Extencoes'' 
{(( 
public)) 

static)) 
class)) 
ExtMDFeConsSitMDFe)) *
{** 
public++ 
static++ 
void++ 
ValidarSchema++ (
(++( )
this++) -
MDFeConsSitMDFe++. =
consSitMdfe++> I
)++I J
{,, 	
var-- 
xmlEnvio-- 
=-- 
consSitMdfe-- &
.--& '
	XmlString--' 0
(--0 1
)--1 2
;--2 3
	Validador// 
.// 
Valida// 
(// 
xmlEnvio// %
,//% &
$str//' >
)//> ?
;//? @
}00 	
public22 
static22 
string22 
	XmlString22 &
(22& '
this22' +
MDFeConsSitMDFe22, ;
consSitMdfe22< G
)22G H
{33 	
return44 

FuncoesXml44 
.44 
ClasseParaXmlString44 1
(441 2
consSitMdfe442 =
)44= >
;44> ?
}55 	
public77 
static77 
XmlDocument77 !
CriaRequestWs77" /
(77/ 0
this770 4
MDFeConsSitMDFe775 D
consSitMdfe77E P
)77P Q
{88 	
var99 
request99 
=99 
new99 
XmlDocument99 )
(99) *
)99* +
;99+ ,
request:: 
.:: 
LoadXml:: 
(:: 
consSitMdfe:: '
.::' (
	XmlString::( 1
(::1 2
)::2 3
)::3 4
;::4 5
return<< 
request<< 
;<< 
}== 	
public?? 
static?? 
void?? 
SalvarXmlEmDisco?? +
(??+ ,
this??, 0
MDFeConsSitMDFe??1 @
consSitMdfe??A L
)??L M
{@@ 	
ifAA 
(AA 
MDFeConfiguracaoAA  
.AA  !
NaoSalvarXmlAA! -
(AA- .
)AA. /
)AA/ 0
returnAA1 7
;AA7 8
varCC 

caminhoXmlCC 
=CC 
MDFeConfiguracaoCC -
.CC- .
CaminhoSalvarXmlCC. >
;CC> ?
varEE 
arquivoSalvarEE 
=EE 

caminhoXmlEE  *
+EE+ ,
$strEE- 1
+EE2 3
consSitMdfeEE4 ?
.EE? @
ChMDFeEE@ F
+EEG H
$strEEI W
;EEW X

FuncoesXmlGG 
.GG  
ClasseParaArquivoXmlGG +
(GG+ ,
consSitMdfeGG, 7
,GG7 8
arquivoSalvarGG9 F
)GGF G
;GGG H
}HH 	
}II 
}JJ Ö
_C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeConsStatServMDFe.cs
	namespace'' 	
MDFe''
 
.'' 
Utils'' 
.'' 
	Extencoes'' 
{(( 
public)) 

static)) 
class)) #
ExtMDFeConsStatServMDFe)) /
{** 
public++ 
static++ 
void++ 
ValidarSchema++ (
(++( )
this++) - 
MDFeConsStatServMDFe++. B
consStatServMDFe++C S
)++S T
{,, 	
var-- 
xmlValidacao-- 
=-- 
consStatServMDFe-- /
.--/ 0
	XmlString--0 9
(--9 :
)--: ;
;--; <
	Validador// 
.// 
Valida// 
(// 
xmlValidacao// )
,//) *
$str//+ G
)//G H
;//H I
}00 	
public22 
static22 
string22 
	XmlString22 &
(22& '
this22' + 
MDFeConsStatServMDFe22, @
consStatServMDFe22A Q
)22Q R
{33 	
return44 

FuncoesXml44 
.44 
ClasseParaXmlString44 1
(441 2
consStatServMDFe442 B
)44B C
;44C D
}55 	
public77 
static77 
XmlDocument77 !
CriaRequestWs77" /
(77/ 0
this770 4 
MDFeConsStatServMDFe775 I
consStatServMdFe77J Z
)77Z [
{88 	
var99 
request99 
=99 
new99 
XmlDocument99 )
(99) *
)99* +
;99+ ,
request:: 
.:: 
LoadXml:: 
(:: 
consStatServMdFe:: ,
.::, -
	XmlString::- 6
(::6 7
)::7 8
)::8 9
;::9 :
return<< 
request<< 
;<< 
}== 	
public?? 
static?? 
void?? 
SalvarXmlEmDisco?? +
(??+ ,
this??, 0 
MDFeConsStatServMDFe??1 E
consStatServMdFe??F V
)??V W
{@@ 	
ifAA 
(AA 
MDFeConfiguracaoAA  
.AA  !
NaoSalvarXmlAA! -
(AA- .
)AA. /
)AA/ 0
returnAA1 7
;AA7 8
varCC 

caminhoXmlCC 
=CC 
MDFeConfiguracaoCC -
.CC- .
CaminhoSalvarXmlCC. >
;CC> ?
varEE 
arquivoSalvarEE 
=EE 

caminhoXmlEE  *
+EE+ ,
$strEE- K
;EEK L

FuncoesXmlGG 
.GG  
ClasseParaArquivoXmlGG +
(GG+ ,
consStatServMdFeGG, <
,GG< =
arquivoSalvarGG> K
)GGK L
;GGL M
}HH 	
}II 
}JJ À
\C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeCosMDFeNaoEnc.cs
	namespace'' 	
MDFe''
 
.'' 
Utils'' 
.'' 
	Extencoes'' 
{(( 
public)) 

static)) 
class))  
ExtMDFeCosMDFeNaoEnc)) ,
{** 
public++ 
static++ 
string++ 
	XmlString++ &
(++& '
this++' +
MDFeCosMDFeNaoEnc++, =
consMDFeNaoEnc++> L
)++L M
{,, 	
return-- 

FuncoesXml-- 
.-- 
ClasseParaXmlString-- 1
(--1 2
consMDFeNaoEnc--2 @
)--@ A
;--A B
}.. 	
public00 
static00 
void00 
ValidarSchema00 (
(00( )
this00) -
MDFeCosMDFeNaoEnc00. ?
consMdFeNaoEnc00@ N
)00N O
{11 	
var22 
xmlValidacao22 
=22 
consMdFeNaoEnc22 -
.22- .
	XmlString22. 7
(227 8
)228 9
;229 :
	Validador33 
.33 
Valida33 
(33 
xmlValidacao33 )
,33) *
$str33+ E
)33E F
;33F G
}44 	
public66 
static66 
XmlDocument66 !
CriaRequestWs66" /
(66/ 0
this660 4
MDFeCosMDFeNaoEnc665 F
cosMdFeNaoEnc66G T
)66T U
{77 	
var88 
request88 
=88 
new88 
XmlDocument88 )
(88) *
)88* +
;88+ ,
request99 
.99 
LoadXml99 
(99 
cosMdFeNaoEnc99 )
.99) *
	XmlString99* 3
(993 4
)994 5
)995 6
;996 7
return;; 
request;; 
;;; 
}<< 	
public>> 
static>> 
void>> 
SalvarXmlEmDisco>> +
(>>+ ,
this>>, 0
MDFeCosMDFeNaoEnc>>1 B
cosMdFeNaoEnc>>C P
)>>P Q
{?? 	
if@@ 
(@@ 
MDFeConfiguracao@@  
.@@  !
NaoSalvarXml@@! -
(@@- .
)@@. /
)@@/ 0
return@@1 7
;@@7 8
varBB 

caminhoXmlBB 
=BB 
MDFeConfiguracaoBB -
.BB- .
CaminhoSalvarXmlBB. >
;BB> ?
varDD 
arquivoSalvarDD 
=DD 

caminhoXmlDD  *
+DD+ ,
$strDD- 1
+DD2 3
cosMdFeNaoEncDD4 A
.DDA B
CNPJDDB F
+DDG H
$strDDI W
;DDW X

FuncoesXmlFF 
.FF  
ClasseParaArquivoXmlFF +
(FF+ ,
cosMdFeNaoEncFF, 9
,FF9 :
arquivoSalvarFF; H
)FFH I
;FFI J
}GG 	
}HH 
}II ∂
WC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeEnviMDFe.cs
	namespace(( 	
MDFe((
 
.(( 
Utils(( 
.(( 
	Extencoes(( 
{)) 
public** 

static** 
class** 
ExtMDFeEnviMDFe** '
{++ 
public,, 
static,, 
void,, 
Valida,, !
(,,! "
this,," &
MDFeEnviMDFe,,' 3
enviMDFe,,4 <
),,< =
{-- 	
if.. 
(.. 
enviMDFe.. 
==.. 
null..  
)..  !
throw.." '
new..( +
ArgumentException.., =
(..= >
$str..> f
)..f g
;..g h
var00 
xmlMdfe00 
=00 

FuncoesXml00 $
.00$ %
ClasseParaXmlString00% 8
(008 9
enviMDFe009 A
)00A B
;00B C
	Validador22 
.22 
Valida22 
(22 
xmlMdfe22 $
,22$ %
$str22& :
)22: ;
;22; <
enviMDFe44 
.44 
MDFe44 
.44 
Valida44  
(44  !
)44! "
;44" #
}55 	
public77 
static77 
XmlDocument77 !
CriaXmlRequestWs77" 2
(772 3
this773 7
MDFeEnviMDFe778 D
enviMDFe77E M
)77M N
{88 	
var99 

dadosEnvio99 
=99 
new99  
XmlDocument99! ,
(99, -
)99- .
;99. /

dadosEnvio:: 
.:: 
LoadXml:: 
(:: 
enviMDFe:: '
.::' (
	XmlString::( 1
(::1 2
)::2 3
)::3 4
;::4 5
return<< 

dadosEnvio<< 
;<< 
}== 	
public?? 
static?? 
string?? 
	XmlString?? &
(??& '
this??' +
MDFeEnviMDFe??, 8
enviMDFe??9 A
)??A B
{@@ 	
varAA 
	xmlStringAA 
=AA 

FuncoesXmlAA &
.AA& '
ClasseParaXmlStringAA' :
(AA: ;
enviMDFeAA; C
)AAC D
;AAD E
returnCC 
	xmlStringCC 
;CC 
}DD 	
publicFF 
staticFF 
voidFF 
SalvarXmlEmDiscoFF +
(FF+ ,
thisFF, 0
MDFeEnviMDFeFF1 =
enviMDFeFF> F
)FFF G
{GG 	
ifHH 
(HH 
MDFeConfiguracaoHH  
.HH  !
NaoSalvarXmlHH! -
(HH- .
)HH. /
)HH/ 0
returnHH1 7
;HH7 8
varJJ 

caminhoXmlJJ 
=JJ 
MDFeConfiguracaoJJ -
.JJ- .
CaminhoSalvarXmlJJ. >
;JJ> ?
varLL 
arquivoSalvarLL 
=LL 

caminhoXmlLL  *
+LL+ ,
$strLL- 1
+LL2 3
enviMDFeLL4 <
.LL< =
MDFeLL= A
.LLA B
ChaveLLB G
(LLG H
)LLH I
+LLJ K
$strLLL `
;LL` a

FuncoesXmlNN 
.NN  
ClasseParaArquivoXmlNN +
(NN+ ,
enviMDFeNN, 4
,NN4 5
arquivoSalvarNN6 C
)NNC D
;NND E
enviMDFePP 
.PP 
MDFePP 
.PP 
SalvarXmlEmDiscoPP *
(PP* +
)PP+ ,
;PP, -
}QQ 	
}RR 
}SS ı	
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeEvCancMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' 
ExtMDFeEvCancMDFe'' )
{(( 
public)) 
static)) 
void)) 
ValidaSchema)) '
())' (
this))( ,
MDFeEvCancMDFe))- ;

evCancMDFe))< F
)))F G
{** 	
var++ 
xmlCancelamento++ 
=++  !

evCancMDFe++" ,
.++, -
	XmlString++- 6
(++6 7
)++7 8
;++8 9
	Validador-- 
.-- 
Valida-- 
(-- 
xmlCancelamento-- ,
,--, -
$str--. D
)--D E
;--E F
}.. 	
public00 
static00 
string00 
	XmlString00 &
(00& '
this00' +
MDFeEvCancMDFe00, :

evCancMDFe00; E
)00E F
{11 	
return22 

FuncoesXml22 
.22 
ClasseParaXmlString22 1
(221 2

evCancMDFe222 <
)22< =
;22= >
}33 	
}44 
}55 Ì	
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeEvEncMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' 
ExtMDFeEvEncMDFe'' (
{(( 
public)) 
static)) 
void)) 
ValidaSchema)) '
())' (
this))( ,
MDFeEvEncMDFe))- :
	evEncMDFe)); D
)))D E
{** 	
var++ 
xmlEncerramento++ 
=++  !
	evEncMDFe++" +
.+++ ,
	XmlString++, 5
(++5 6
)++6 7
;++7 8
	Validador-- 
.-- 
Valida-- 
(-- 
xmlEncerramento-- ,
,--, -
$str--. C
)--C D
;--D E
}.. 	
public00 
static00 
string00 
	XmlString00 &
(00& '
this00' +
MDFeEvEncMDFe00, 9
	evEncMDFe00: C
)00C D
{11 	
return22 

FuncoesXml22 
.22 
ClasseParaXmlString22 1
(221 2
	evEncMDFe222 ;
)22; <
;22< =
}33 	
}44 
}55 Ω*
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeEventoMDFe.cs
	namespace)) 	
MDFe))
 
.)) 
Utils)) 
.)) 
	Extencoes)) 
{** 
public++ 

static++ 
class++ 
ExtMDFeEventoMDFe++ )
{,, 
public-- 
static-- 
void-- 
ValidarSchema-- (
(--( )
this--) -
MDFeEventoMDFe--. <
evento--= C
)--C D
{.. 	
var// 
	xmlValido// 
=// 
evento// "
.//" #
	XmlString//# ,
(//, -
)//- .
;//. /
	Validador11 
.11 
Valida11 
(11 
	xmlValido11 &
,11& '
$str11( >
)11> ?
;11? @
var33 

tipoEvento33 
=33 
evento33 #
.33# $
	InfEvento33$ -
.33- .
	DetEvento33. 7
.337 8
EventoContainer338 G
.33G H
GetType33H O
(33O P
)33P Q
;33Q R
if55 
(55 

tipoEvento55 
==55 
typeof55 $
(55$ %
MDFeEvCancMDFe55% 3
)553 4
)554 5
{66 
var77 
	objetoXml77 
=77 
(77  !
MDFeEvCancMDFe77! /
)77/ 0
evento770 6
.776 7
	InfEvento777 @
.77@ A
	DetEvento77A J
.77J K
EventoContainer77K Z
;77Z [
	objetoXml88 
.88 
ValidaSchema88 &
(88& '
)88' (
;88( )
}99 
if;; 
(;; 

tipoEvento;; 
==;; 
typeof;; $
(;;$ %
MDFeEvEncMDFe;;% 2
);;2 3
);;3 4
{<< 
var== 
	objetoXml== 
=== 
(==  !
MDFeEvEncMDFe==! .
)==. /
evento==/ 5
.==5 6
	InfEvento==6 ?
.==? @
	DetEvento==@ I
.==I J
EventoContainer==J Y
;==Y Z
	objetoXml?? 
.?? 
ValidaSchema?? &
(??& '
)??' (
;??( )
}@@ 
ifBB 
(BB 

tipoEventoBB 
==BB 
typeofBB $
(BB$ %!
MDFeEvIncCondutorMDFeBB% :
)BB: ;
)BB; <
{CC 
varDD 
	objetoXmlDD 
=DD 
(DD  !!
MDFeEvIncCondutorMDFeDD! 6
)DD6 7
eventoDD7 =
.DD= >
	InfEventoDD> G
.DDG H
	DetEventoDDH Q
.DDQ R
EventoContainerDDR a
;DDa b
	objetoXmlFF 
.FF 
ValidaSchemaFF &
(FF& '
)FF' (
;FF( )
}GG 
}JJ 	
publicLL 
staticLL 
XmlDocumentLL !
CriaXmlRequestWsLL" 2
(LL2 3
thisLL3 7
MDFeEventoMDFeLL8 F
eventoLLG M
)LLM N
{MM 	
varNN 

xmlRequestNN 
=NN 
newNN  
XmlDocumentNN! ,
(NN, -
)NN- .
;NN. /

xmlRequestOO 
.OO 
LoadXmlOO 
(OO 
eventoOO %
.OO% &
	XmlStringOO& /
(OO/ 0
)OO0 1
)OO1 2
;OO2 3
returnQQ 

xmlRequestQQ 
;QQ 
}RR 	
publicTT 
staticTT 
stringTT 
	XmlStringTT &
(TT& '
thisTT' +
MDFeEventoMDFeTT, :
eventoTT; A
)TTA B
{UU 	
returnVV 

FuncoesXmlVV 
.VV 
ClasseParaXmlStringVV 1
(VV1 2
eventoVV2 8
)VV8 9
;VV9 :
}WW 	
publicYY 
staticYY 
voidYY 
AssinarYY "
(YY" #
thisYY# '
MDFeEventoMDFeYY( 6
eventoYY7 =
)YY= >
{ZZ 	
evento[[ 
.[[ 
	Signature[[ 
=[[ 
AssinaturaDigital[[ 0
.[[0 1
Assina[[1 7
([[7 8
evento[[8 >
,[[> ?
evento[[@ F
.[[F G
	InfEvento[[G P
.[[P Q
Id[[Q S
,[[S T
MDFeConfiguracao\\  
.\\  !
X509Certificate2\\! 1
)\\1 2
;\\2 3
}]] 	
public__ 
static__ 
void__ 
SalvarXmlEmDisco__ +
(__+ ,
this__, 0
MDFeEventoMDFe__1 ?
evento__@ F
,__F G
string__H N
chave__O T
)__T U
{`` 	
ifaa 
(aa 
MDFeConfiguracaoaa  
.aa  !
NaoSalvarXmlaa! -
(aa- .
)aa. /
)aa/ 0
returnaa1 7
;aa7 8
varcc 

caminhoXmlcc 
=cc 
MDFeConfiguracaocc -
.cc- .
CaminhoSalvarXmlcc. >
;cc> ?
varee 
arquivoSalvaree 
=ee 

caminhoXmlee  *
+ee+ ,
$stree- 1
+ee2 3
chaveee4 9
+ee: ;
$stree< J
;eeJ K

FuncoesXmlgg 
.gg  
ClasseParaArquivoXmlgg +
(gg+ ,
eventogg, 2
,gg2 3
arquivoSalvargg4 A
)ggA B
;ggB C
}hh 	
}jj 
}kk ≥

`C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeEvIncCondutorMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' $
ExtMDFeEvIncCondutorMDFe'' 0
{(( 
public)) 
static)) 
void)) 
ValidaSchema)) '
())' (
this))( ,!
MDFeEvIncCondutorMDFe))- B
evIncCondutorMDFe))C T
)))T U
{** 	
var++ 
xmlIncluirCondutor++ "
=++# $
evIncCondutorMDFe++% 6
.++6 7
	XmlString++7 @
(++@ A
)++A B
;++B C
	Validador-- 
.-- 
Valida-- 
(-- 
xmlIncluirCondutor-- /
,--/ 0
$str--1 N
)--N O
;--O P
}.. 	
public00 
static00 
string00 
	XmlString00 &
(00& '
this00' +!
MDFeEvIncCondutorMDFe00, A
evIncCondutorMDFe00B S
)00S T
{11 	
return22 

FuncoesXml22 
.22 
ClasseParaXmlString22 1
(221 2
evIncCondutorMDFe222 C
)22C D
;22D E
}33 	
}44 
}55 ƒ

]C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetConsMDFeNao.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' !
ExtMDFeRetConsMDFeNao'' -
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetConsMDFeNao))1 C
retConsMdFeNao))D R
,))R S
string))T Z
cnpj))[ _
)))_ `
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- 1
+//2 3
cnpj//4 8
+//9 :
$str//; E
;//E F

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
retConsMdFeNao11, :
,11: ;
arquivoSalvar11< I
)11I J
;11J K
}22 	
}33 
}44 ∑

^C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetConsReciMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' "
ExtMDFeRetConsReciMDFe'' .
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetConsReciMDFe))1 D
consReciMdFe))E Q
)))Q R
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- 1
+//2 3
consReciMdFe//4 @
.//@ A
NRec//A E
+//F G
$str//H V
;//V W

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
consReciMdFe11, 8
,118 9
arquivoSalvar11: G
)11G H
;11H I
}22 	
}33 
}44 ∆

]C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetConsSitMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' !
ExtMDFeRetConsSitMDFe'' -
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetConsSitMDFe))1 C
retConsSitMdFe))D R
,))R S
string))T Z
chave))[ `
)))` a
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- 1
+//2 3
chave//4 9
+//: ;
$str//< F
;//F G

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
retConsSitMdFe11, :
,11: ;
arquivoSalvar11< I
)11I J
;11J K
}22 	
}33 
}44 “	
^C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetConsStatServ.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' "
ExtMDFeRetConsStatServ'' .
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetConsStatServ))1 D
retConsStatServ))E T
)))T U
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- L
;//L M

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
retConsStatServ11, ;
,11; <
arquivoSalvar11= J
)11J K
;11K L
}22 	
}33 
}44 À

ZC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetEnviMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class'' 
ExtMDFeRetEnviMDFe'' *
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetEnviMDFe))1 @
retEnviMDFe))A L
)))L M
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- 1
+//2 3
retEnviMDFe//4 ?
.//? @
InfRec//@ F
.//F G
NRec//G K
+//L M
$str//N X
;//X Y

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
retEnviMDFe11, 7
,117 8
arquivoSalvar119 F
)11F G
;11G H
}22 	
}33 
}44 π

\C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtMDFeRetEventoMDFe.cs
	namespace%% 	
MDFe%%
 
.%% 
Utils%% 
.%% 
	Extencoes%% 
{&& 
public'' 

static'' 
class''  
ExtMDFeRetEventoMDFe'' ,
{(( 
public)) 
static)) 
void)) 
SalvarXmlEmDisco)) +
())+ ,
this)), 0
MDFeRetEventoMDFe))1 B
	retEvento))C L
,))L M
string))N T
chave))U Z
)))Z [
{** 	
if++ 
(++ 
MDFeConfiguracao++  
.++  !
NaoSalvarXml++! -
(++- .
)++. /
)++/ 0
return++1 7
;++7 8
var-- 

caminhoXml-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSalvarXml--. >
;--> ?
var// 
arquivoSalvar// 
=// 

caminhoXml//  *
+//+ ,
$str//- 1
+//2 3
chave//4 9
+//: ;
$str//< F
;//F G

FuncoesXml11 
.11  
ClasseParaArquivoXml11 +
(11+ ,
	retEvento11, 5
,115 6
arquivoSalvar117 D
)11D E
;11E F
}22 	
}33 
}44 «	
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Extencoes\ExtVersaoServico.cs
	namespace## 	
MDFe##
 
.## 
Utils## 
.## 
	Extencoes## 
{$$ 
public%% 

static%% 
class%% 
ExtVersaoServico%% (
{&& 
public'' 
static'' 
string'' 
GetVersaoString'' ,
('', -
this''- 1
VersaoServico''2 ?
versaoServico''@ M
)''M N
{(( 	
var)) 
codigoString)) 
=)) 
versaoServico)) ,
.)), -
ToString))- 5
())5 6
)))6 7
;))7 8
var** 
codigoFormatado** 
=**  !
codigoString**" .
.**. /
	Substring**/ 8
(**8 9
$num**9 :
,**: ;
$num**< =
)**= >
;**> ?
codigoFormatado++ 
=++ 
codigoFormatado++ -
.++- .
Insert++. 4
(++4 5
$num++5 6
,++6 7
$str++8 ;
)++; <
;++< =
return,, 
codigoFormatado,, "
;,," #
}-- 	
}.. 
}// Ä
UC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str %
)% &
]& '
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
$str '
)' (
]( )
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
]##) *–
QC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Utils\Validacao\Validador.cs
	namespace'' 	
MDFe''
 
.'' 
Utils'' 
.'' 
	Validacao'' 
{(( 
public)) 

class)) 
	Validador)) 
{** 
public++ 
static++ 
void++ 
Valida++ !
(++! "
string++" (
xml++) ,
,++, -
string++. 4
schema++5 ;
)++; <
{,, 	
var-- 

pathSchema-- 
=-- 
MDFeConfiguracao-- -
.--- .
CaminhoSchemas--. <
;--< =
if// 
(// 
!// 
	Directory// 
.// 
Exists// !
(//! "

pathSchema//" ,
)//, -
)//- .
throw00 
new00 
	Exception00 #
(00# $
$str00$ M
+00N O

pathSchema00P Z
)00Z [
;00[ \
var22 
arquivoSchema22 
=22 

pathSchema22  *
+22+ ,
$str22- 1
+222 3
schema224 :
;22: ;
var55 
cfg55 
=55 
new55 
XmlReaderSettings55 +
{55, -
ValidationType55. <
=55= >
ValidationType55? M
.55M N
Schema55N T
}55U V
;55V W
var88 
schemas88 
=88 
new88 
XmlSchemaSet88 *
(88* +
)88+ ,
;88, -
cfg99 
.99 
Schemas99 
=99 
schemas99 !
;99! "
schemas<< 
.<< 
Add<< 
(<< 
null<< 
,<< 
arquivoSchema<< +
)<<+ ,
;<<, -
cfg>> 
.>> "
ValidationEventHandler>> &
+=>>' )"
ValidationEventHandler>>* @
;>>@ A
var@@ 
	validator@@ 
=@@ 
	XmlReader@@ %
.@@% &
Create@@& ,
(@@, -
new@@- 0
StringReader@@1 =
(@@= >
xml@@> A
)@@A B
,@@B C
cfg@@D G
)@@G H
;@@H I
tryAA 
{BB 
whileDD 
(DD 
	validatorDD  
.DD  !
ReadDD! %
(DD% &
)DD& '
)DD' (
{EE 
}FF 
}GG 
catchHH 
(HH 
XmlExceptionHH 
errHH  #
)HH# $
{II 
throwLL 
newLL 
	ExceptionLL #
(LL# $
$strLL$ V
+LLW X
$strLLY ]
+LL^ _
errLL` c
.LLc d
MessageLLd k
)LLk l
;LLl m
}MM 
finallyNN 
{OO 
	validatorPP 
.PP 
ClosePP 
(PP  
)PP  !
;PP! "
}QQ 
}RR 	
privateTT 
staticTT 
voidTT "
ValidationEventHandlerTT 2
(TT2 3
objectTT3 9
senderTT: @
,TT@ A
ValidationEventArgsTTB U
eTTV W
)TTW X
{UU 	
throwVV 
newVV 
	ExceptionVV 
(VV  
$strVV  7
+VV8 9
eVV: ;
.VV; <
MessageVV< C
)VVC D
;VVD E
}WW 	
}XX 
}YY 
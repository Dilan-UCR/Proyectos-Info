É*
?C:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. 
	Configure 
< 
KafkaOptions '
>' (
(( )
builder) 0
.0 1
Configuration1 >
.> ?

GetSection? I
(I J
$strJ Q
)Q R
)R S
;S T
builder 
. 
Services 
. 
	AddScoped 
< 
IReportJobService ,
,, -
ReportJobService. >
>> ?
(? @
)@ A
;A B
builder 
. 
Services 
. 
AddSingleton 
< !
IKafkaProducerService 3
,3 4 
KafkaProducerService5 I
>I J
(J K
)K L
;L M
builder 
. 
Services 
. 
	AddScoped 
< 
IEmailJobService +
,+ ,
EmailJobService- <
>< =
(= >
)> ?
;? @
builder 
. 
Services 
. 
	AddScoped 
<  
IMessagingJobService /
,/ 0
MessagingJobService1 D
>D E
(E F
)F G
;G H
builder 
. 
Services 
. 
	AddScoped 
< 
IHttpClientService -
,- .
HttpClientService/ @
>@ A
(A B
)B C
;C D
builder 
. 
Services 
. 
	Configure 
< 
PdfServerOptions +
>+ ,
(, -
builder- 4
.4 5
Configuration5 B
.B C

GetSectionC M
(M N
$strN Y
)Y Z
)Z [
;[ \
builder 
. 
Services 
. 
AddHttpClient 
< 
IHttpClientService 1
,1 2
HttpClientService3 D
>D E
(E F
clientF L
=>M O
{ 
client 

.
 
Timeout 
= 
TimeSpan 
. 
FromSeconds )
() *
$num* ,
), -
;- .
client 

.
 !
DefaultRequestHeaders  
.  !
Add! $
($ %
$str% -
,- .
$str/ A
)A B
;B C
} 
) 
; 
builder 
. 
Services 
. 
AddHangfire 
( 
config #
=>$ &
config 

.
 %
SetDataCompatibilityLevel $
($ %
CompatibilityLevel% 7
.7 8
Version_1708 C
)C D
.
 /
#UseSimpleAssemblyNameTypeSerializer .
(. /
)/ 0
.
 ,
 UseRecommendedSerializerSettings +
(+ ,
), -
.  
 
UseSqlServerStorage   
(   
builder!! 
.!! 
Configuration!! #
.!!# $
GetConnectionString!!$ 7
(!!7 8
$str!!8 H
)!!H I
,!!I J
new"" #
SqlServerStorageOptions"" )
{## "
CommandBatchMaxTimeout$$ (
=$$) *
TimeSpan$$+ 3
.$$3 4
FromMinutes$$4 ?
($$? @
$num$$@ A
)$$A B
,$$B C&
SlidingInvisibilityTimeout%% ,
=%%- .
TimeSpan%%/ 7
.%%7 8
FromMinutes%%8 C
(%%C D
$num%%D E
)%%E F
,%%F G
QueuePollInterval&& #
=&&$ %
TimeSpan&&& .
.&&. /
Zero&&/ 3
,&&3 4(
UseRecommendedIsolationLevel'' .
=''/ 0
true''1 5
,''5 6
DisableGlobalLocks(( $
=((% &
true((' +
})) 
)**
 
)++ 
;++ 
builder-- 
.-- 
Services-- 
.-- 
AddHangfireServer-- "
(--" #
)--# $
;--$ %
var// 
app// 
=// 	
builder//
 
.// 
Build// 
(// 
)// 
;// 
app22 
.22 

UseRouting22 
(22 
)22 
;22 
app33 
.33 
UseAuthorization33 
(33 
)33 
;33 
app66 
.66 
MapControllers66 
(66 
)66 
;66 
app77 
.77  
UseHangfireDashboard77 
(77 
$str77 $
)77$ %
;77% &
await99 
app99 	
.99	 

RunAsync99
 
(99 
)99 
;99 ◊
_C:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Validation\ReportRequestValidator.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 

Validation )
{ 
public 

static 
class "
ReportRequestValidator .
{ 
public 
static 
bool 
IsValid "
(" #
PdfRequestDto# 0
request1 8
,8 9
out: =
string> D
errorMessageE Q
)Q R
{ 	
errorMessage		 
=		 
string		 !
.		! "
Empty		" '
;		' (
if 
( 
request 
. 

CustomerId "
<=# %
$num& '
)' (
{ 
errorMessage 
= 
$str A
;A B
return 
false 
; 
} 
if 
( 
request 
. 
	StartDate !
>=" $
request% ,
., -
EndDate- 4
)4 5
{ 
errorMessage 
= 
$str C
;C D
return 
false 
; 
} 
if 
( 
request 
. 
Products  
==! #
null$ (
||) +
!, -
request- 4
.4 5
Products5 =
.= >
Any> A
(A B
)B C
)C D
{ 
errorMessage 
= 
$str B
;B C
return 
false 
; 
} 
return 
true 
; 
} 	
} 
}   Ω
WC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\PdfServerOptions.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{ 
public 

class 
PdfServerOptions !
{ 
public 
string 
BaseUrl 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
} 
} Ø)
[C:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\KafkaProducerService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{ 
public		 

class		 
KafkaOptions		 
{

 
public 
string 
BootstrapServers &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
=5 6
$str7 L
;L M
public 
string 
Topic 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$str, ;
;; <
} 
public 

class  
KafkaProducerService %
:& '!
IKafkaProducerService( =
,= >
IDisposable? J
{ 
private 
readonly 
	IProducer "
<" #
Null# '
,' (
string) /
>/ 0
	_producer1 :
;: ;
private 
readonly 
string 
_topic  &
;& '
private 
readonly 
ILogger  
<  ! 
KafkaProducerService! 5
>5 6
_logger7 >
;> ?
private 
bool 
	_disposed 
; 
public  
KafkaProducerService #
(# $
IOptions$ ,
<, -
KafkaOptions- 9
>9 :
options; B
,B C
ILoggerD K
<K L 
KafkaProducerServiceL `
>` a
loggerb h
)h i
{ 	
var 
config 
= 
new 
ProducerConfig +
{ 
BootstrapServers  
=! "
options# *
.* +
Value+ 0
.0 1
BootstrapServers1 A
,A B
MessageTimeoutMs  
=! "
$num# '
} 
; 
	_producer 
= 
new 
ProducerBuilder +
<+ ,
Null, 0
,0 1
string2 8
>8 9
(9 :
config: @
)@ A
.A B
BuildB G
(G H
)H I
;I J
_topic 
= 
options 
. 
Value "
." #
Topic# (
;( )
_logger   
=   
logger   
;   
}!! 	
public## 
async## 
Task## 
<## 
bool## 
>## 
SendLogAsync##  ,
(##, -
LogRequestDto##- :
log##; >
)##> ?
{$$ 	
try%% 
{&& 
var'' 
message'' 
='' 
JsonSerializer'' ,
.'', -
	Serialize''- 6
(''6 7
log''7 :
)'': ;
;''; <
await)) 
	_producer)) 
.))  
ProduceAsync))  ,
()), -
_topic))- 3
,))3 4
new))5 8
Message))9 @
<))@ A
Null))A E
,))E F
string))G M
>))M N
{))O P
Value))Q V
=))W X
message))Y `
}))a b
)))b c
;))c d
_logger++ 
.++ 
LogInformation++ &
(++& '
$str++' [
,++[ \
log++] `
.++` a
CorrelationId++a n
)++n o
;++o p
return,, 
true,, 
;,, 
}-- 
catch.. 
(.. 
	Exception.. 
ex.. 
)..  
{// 
_logger00 
.00 
LogError00  
(00  !
ex00! #
,00# $
$str00% a
,00a b
log00c f
.00f g
CorrelationId00g t
)00t u
;00u v
return11 
false11 
;11 
}22 
}33 	
	protected55 
virtual55 
void55 
Dispose55 &
(55& '
bool55' +
	disposing55, 5
)555 6
{66 	
if77 
(77 
!77 
	_disposed77 
)77 
{88 
if99 
(99 
	disposing99 
)99 
{:: 
	_producer;; 
?;; 
.;; 
Flush;; $
(;;$ %
TimeSpan;;% -
.;;- .
FromSeconds;;. 9
(;;9 :
$num;;: ;
);;; <
);;< =
;;;= >
	_producer<< 
?<< 
.<< 
Dispose<< &
(<<& '
)<<' (
;<<( )
}== 
	_disposed?? 
=?? 
true??  
;??  !
}@@ 
}AA 	
publicCC 
voidCC 
DisposeCC 
(CC 
)CC 
{DD 	
DisposeEE 
(EE 
trueEE 
)EE 
;EE 
GCFF 
.FF 
SuppressFinalizeFF 
(FF  
thisFF  $
)FF$ %
;FF% &
}GG 	
}HH 
}II ﬁ-
WC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\ReportJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{ 
public 

class 
ReportJobService !
:" #
IReportJobService$ 5
{		 
private

 
readonly

 
IHttpClientService

 +
_httpClient

, 7
;

7 8
private 
readonly !
IKafkaProducerService .
_kafkaProducer/ =
;= >
private 
readonly 
ILogger  
<  !
ReportJobService! 1
>1 2
_logger3 :
;: ;
public 
ReportJobService 
(  
IHttpClientService 

httpClient )
,) *!
IKafkaProducerService !
kafkaProducer" /
,/ 0
ILogger 
< 
ReportJobService $
>$ %
logger& ,
), -
{ 	
_httpClient 
= 

httpClient $
;$ %
_kafkaProducer 
= 
kafkaProducer *
;* +
_logger 
= 
logger 
; 
} 	
[ 	
AutomaticRetry	 
( 
Attempts  
=! "
$num# $
)$ %
]% &
public 
async 
Task  
ProcessReportRequest .
(. /
int/ 2

customerId3 =
,= >
DateTime? G
	startDateH Q
,Q R
DateTimeS [
endDate\ c
,c d
stringe k
correlationIdl y
,y z
List{ 
<	 Ä
int
Ä É
>
É Ñ
products
Ö ç
)
ç é
{ 	
var 
log 
= 
new 
LogRequestDto '
{ 
CorrelationId 
= 
correlationId  -
,- .
Service 
= 
$str *
,* +
Endpoint 
= 
$str 1
,1 2
	Timestamp   
=   
DateTime   $
.  $ %
UtcNow  % +
,  + ,
Payload!! 
=!! 
$"!! 
$str!! (
{!!( )

customerId!!) 3
}!!3 4
$str!!4 A
{!!A B
	startDate!!B K
:!!K L
$str!!L V
}!!V W
$str!!W b
{!!b c
endDate!!c j
:!!j k
$str!!k u
}!!u v
"!!v w
,!!w x
Success"" 
="" 
false"" 
}## 
;## 
log$$ 
.$$ 
Payload$$ 
+=$$ 
$"$$ 
$str$$ *
{$$* +
string$$+ 1
.$$1 2
Join$$2 6
($$6 7
$str$$7 ;
,$$; <
products$$= E
)$$E F
}$$F G
"$$G H
;$$H I
try&& 
{'' 
_logger(( 
.(( 
LogInformation(( &
(((& '
$str((' _
,((_ `
correlationId((a n
)((n o
;((o p
var++ 

pdfRequest++ 
=++  
new++! $
PdfRequestDto++% 2
{,, 

CustomerId-- 
=--  

customerId--! +
,--+ ,
	StartDate.. 
=.. 
	startDate..  )
,..) *
EndDate// 
=// 
endDate// %
,//% &
CorrelationId00 !
=00" #
correlationId00$ 1
,001 2
Products11 
=11 
products11 '
}22 
;22 
var55 
success55 
=55 
await55 #
_httpClient55$ /
.55/ 0"
SendReportRequestAsync550 F
(55F G

pdfRequest55G Q
)55Q R
;55R S
log77 
.77 
Success77 
=77 
success77 %
;77% &
log88 
.88 
Payload88 
+=88 
$"88 !
$str88! 6
{886 7
(887 8
success888 ?
?88@ A
$str88B K
:88L M
$str88N V
)88V W
}88W X
"88X Y
;88Y Z
await;; 
_kafkaProducer;; $
.;;$ %
SendLogAsync;;% 1
(;;1 2
log;;2 5
);;5 6
;;;6 7
if== 
(== 
success== 
)== 
{>> 
_logger?? 
.?? 
LogInformation?? *
(??* +
$str??+ d
,??d e
correlationId??f s
)??s t
;??t u
}@@ 
elseAA 
{BB 
_loggerCC 
.CC 

LogWarningCC &
(CC& '
$strCC' l
,CCl m
correlationIdCCn {
)CC{ |
;CC| }
}DD 
}EE 
catchFF 
(FF 
	ExceptionFF 
exFF 
)FF  
{GG 
logHH 
.HH 
PayloadHH 
+=HH 
$"HH !
$strHH! *
{HH* +
exHH+ -
.HH- .
MessageHH. 5
}HH5 6
"HH6 7
;HH7 8
awaitII 
_kafkaProducerII $
.II$ %
SendLogAsyncII% 1
(II1 2
logII2 5
)II5 6
;II6 7
_loggerJJ 
.JJ 
LogErrorJJ  
(JJ  !
exJJ! #
,JJ# $
$strJJ% V
,JJV W
correlationIdJJX e
)JJe f
;JJf g
throwKK 
;KK 
}LL 
}MM 	
}NN 
}OO ø)
ZC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\MessagingJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{ 
public 

class 
MessagingJobService $
:% & 
IMessagingJobService' ;
{ 
private 
readonly 
ILogger  
<  !
MessagingJobService! 4
>4 5
_logger6 =
;= >
private 
readonly 
IHttpClientService +
_httpClientService, >
;> ?
private		 
readonly		 
IConfiguration		 '
_configuration		( 6
;		6 7
public 
MessagingJobService "
(" #
ILogger# *
<* +
MessagingJobService+ >
>> ?
logger@ F
,F G
IHttpClientServiceH Z
httpClientService[ l
,l m
IConfigurationn |
configuration	} ä
)
ä ã
{ 	
_logger 
= 
logger 
; 
_httpClientService 
=  
httpClientService! 2
;2 3
_configuration 
= 
configuration *
;* +
} 	
public 
async 
Task 
SendMessageAsync *
(* +
string+ 1
correlationId2 ?
,? @
stringA G
chatIdH N
,N O
stringP V
platformW _
,_ `
stringa g
messageh o
)o p
{ 	
try 
{ 
_logger 
. 
LogInformation &
(& '
$str	 £
,
£ §
correlationId !
??" $
$str% +
,+ ,
chatId 
?? 
$str $
,$ %
platform 
?? 
$str  &
,& '
message 
?? 
$str %
)% &
;& '
var 
messagingPayload $
=% &
new' *
{ 
CorrelationId !
=" #
correlationId$ 1
,1 2
ChatId   
=   
chatId   #
,  # $
Platform!! 
=!! 
platform!! '
,!!' (
Message"" 
="" 
message"" %
}## 
;## 
var%% 
messagingApiUrl%% #
=%%$ %
_configuration%%& 4
[%%4 5
$str%%5 T
]%%T U
??&& 
throw&& 
new&&  %
InvalidOperationException&&! :
(&&: ;
$str&&; t
)&&t u
;&&u v
var(( 
response(( 
=(( 
await(( $
_httpClientService((% 7
.((7 8
	PostAsync((8 A
(((A B
messagingApiUrl((B Q
,((Q R
messagingPayload((S c
)((c d
;((d e
if** 
(** 
response** 
.** 
IsSuccessStatusCode** 0
)**0 1
{++ 
var,, 
responseContent,, '
=,,( )
await,,* /
response,,0 8
.,,8 9
Content,,9 @
.,,@ A
ReadAsStringAsync,,A R
(,,R S
),,S T
;,,T U
_logger-- 
.-- 
LogInformation-- *
(--* +
$str--+ 
,	-- Ä
correlationId.. %
,..% &
responseContent..' 6
)..6 7
;..7 8
}// 
else00 
{11 
var22 
errorContent22 $
=22% &
await22' ,
response22- 5
.225 6
Content226 =
.22= >
ReadAsStringAsync22> O
(22O P
)22P Q
;22Q R
_logger33 
.33 
LogError33 $
(33$ %
$str	33% É
,
33É Ñ
response44  
.44  !

StatusCode44! +
,44+ ,
errorContent44- 9
,449 :
correlationId44; H
)44H I
;44I J
throw66 
new66  
HttpRequestException66 2
(662 3
$"77 
$str77 1
{771 2
response772 :
.77: ;

StatusCode77; E
}77E F
$str77F H
{77H I
errorContent77I U
}77U V
$str77V f
{77f g
correlationId77g t
}77t u
"77u v
)77v w
;77w x
}88 
}99 
catch:: 
(:: 
	Exception:: 
ex:: 
)::  
{;; 
_logger<< 
.<< 
LogError<<  
(<<  !
ex<<! #
,<<# $
$str<<% ]
,<<] ^
correlationId<<_ l
)<<l m
;<<m n
throw== 
new== %
InvalidOperationException== 3
(==3 4
$">> 
$str>> <
{>>< =
correlationId>>= J
}>>J K
">>K L
,>>L M
ex>>N P
)>>P Q
;>>Q R
}?? 
}@@ 	
}AA 
}BB Ø
cC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\Interfaces\IReportJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
.' (

Interfaces( 2
{ 
public 
	interface 
IReportJobService '
{ 
Task  
ProcessReportRequest !
(! "
int" %

customerId& 0
,0 1
DateTime2 :
	startDate; D
,D E
DateTimeF N
endDateO V
,V W
stringX ^
correlationId_ l
,m n
Listn r
<r s
ints v
>v w
products	x Ä
)
Ä Å
;
Å Ç
} 
} ¿
fC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\Interfaces\IMessagingJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
.' (

Interfaces( 2
{ 
public 

	interface  
IMessagingJobService )
{ 
Task 
SendMessageAsync 
( 
string $
correlationId% 2
,2 3
string4 :
chatId; A
,A B
stringB H
platformI Q
,Q R
stringS Y
messageZ a
)a b
;b c
} 
} √
gC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\Interfaces\IKafkaProducerService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
.' (

Interfaces( 2
{ 
public 
	interface !
IKafkaProducerService -
{ 
Task 
< 
bool 
> 
SendLogAsync 
(  
LogRequestDto  -
log. 1
)1 2
;2 3
} 
}		 ö
dC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\Interfaces\IHttpClientService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
.' (

Interfaces( 2
{ 
public 	
	interface
 
IHttpClientService &
{ 
Task 
< 
bool 
> "
SendReportRequestAsync )
() *
PdfRequestDto* 7
request8 ?
)? @
;@ A
Task 
< 
bool 
> 
SendLogAsync 
(  
LogRequestDto  -
log. 1
)1 2
;2 3
Task		 
<		 
HttpResponseMessage		  
>		  !
	PostAsync		" +
<		+ ,
T		, -
>		- .
(		. /
string		/ 5
url		6 9
,		9 :
T		; <
payload		= D
)		D E
;		E F
}

 
} Ó
bC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\Interfaces\IEmailJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
.' (

Interfaces( 2
{ 
public 

	interface 
IEmailJobService %
{ 
Task 
SendEmailAsync 
( 
string "
correlationId# 0
,0 1
string2 8
toEmail9 @
,@ A
stringB H
subjectI P
,P Q
stringR X
messageY `
,` a
intb e

customerIdf p
)p q
;q r
} 
} ÜB
XC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\HttpClientService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{		 
public

 

class

 
HttpClientService

 "
:

# $
IHttpClientService

% 7
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
private 
readonly !
IKafkaProducerService .
_kafkaProducer/ =
;= >
private 
readonly 
ILogger  
<  !
HttpClientService! 2
>2 3
_logger4 ;
;; <
private 
readonly 
PdfServerOptions )
_pdfOptions* 5
;5 6
public 
HttpClientService  
(  !

HttpClient 

httpClient !
,! "!
IKafkaProducerService !
kafkaProducer" /
,/ 0
ILogger 
< 
HttpClientService %
>% &
logger' -
,- .
	Microsoft 
. 

Extensions  
.  !
Options! (
.( )
IOptions) 1
<1 2
PdfServerOptions2 B
>B C

pdfOptionsD N
)N O
{ 	
_httpClient 
= 

httpClient $
;$ %
_kafkaProducer 
= 
kafkaProducer *
;* +
_logger 
= 
logger 
; 
_pdfOptions 
= 

pdfOptions $
.$ %
Value% *
;* +
} 	
public 
async 
Task 
< 
bool 
> "
SendReportRequestAsync  6
(6 7
PdfRequestDto7 D
requestE L
)L M
{ 	
try 
{   
_logger!! 
.!! 
LogInformation!! &
(!!& '
$str!!' F
,!!F G
_pdfOptions!!H S
.!!S T
BaseUrl!!T [
)!![ \
;!!\ ]
var"" 
response"" 
="" 
await"" $
_httpClient""% 0
.""0 1
PostAsJsonAsync""1 @
(""@ A
_pdfOptions""A L
.""L M
BaseUrl""M T
,""T U
request""V ]
)""] ^
;""^ _
if$$ 
($$ 
response$$ 
.$$ 
IsSuccessStatusCode$$ 0
)$$0 1
{%% 
_logger&& 
.&& 
LogInformation&& *
(&&* +
$str&&+ n
,&&n o
request&&p w
.&&w x
CorrelationId	&&x Ö
)
&&Ö Ü
;
&&Ü á
return'' 
true'' 
;''  
}(( 
else)) 
{** 
_logger++ 
.++ 
LogError++ $
(++$ %
$str++% l
,++l m
response,,  
.,,  !

StatusCode,,! +
,,,+ ,
request,,- 4
.,,4 5
CorrelationId,,5 B
),,B C
;,,C D
return-- 
false--  
;--  !
}.. 
}// 
catch00 
(00 
	Exception00 
ex00 
)00  
{11 
_logger22 
.22 
LogError22  
(22  !
ex22! #
,22# $
$str22% i
,22i j
request22k r
.22r s
CorrelationId	22s Ä
)
22Ä Å
;
22Å Ç
throw33 
new33  
HttpRequestException33 .
(33. /
$"33/ 1
$str331 h
{33h i
request33i p
.33p q
CorrelationId33q ~
}33~ 
"	33 Ä
,
33Ä Å
ex
33Ç Ñ
)
33Ñ Ö
;
33Ö Ü
}44 
}55 	
public77 
async77 
Task77 
<77 
bool77 
>77 
SendLogAsync77  ,
(77, -
LogRequestDto77- :
log77; >
)77> ?
{88 	
return99 
await99 
_kafkaProducer99 '
.99' (
SendLogAsync99( 4
(994 5
log995 8
)998 9
;999 :
}:: 	
public<< 
async<< 
Task<< 
<<< 
HttpResponseMessage<< -
><<- .
	PostAsync<</ 8
(<<8 9
string<<9 ?
url<<@ C
,<<C D
object<<E K
payload<<L S
)<<S T
{== 	
try>> 
{?? 
var@@ 
json@@ 
=@@ 
JsonSerializer@@ )
.@@) *
	Serialize@@* 3
(@@3 4
payload@@4 ;
)@@; <
;@@< =
varAA 
contentAA 
=AA 
newAA !
StringContentAA" /
(AA/ 0
jsonAA0 4
,AA4 5
EncodingAA6 >
.AA> ?
UTF8AA? C
,AAC D
$strAAE W
)AAW X
;AAX Y
_loggerCC 
.CC 
LogInformationCC &
(CC& '
$strCC' >
,CC> ?
urlCC@ C
)CCC D
;CCD E
varEE 
responseEE 
=EE 
awaitEE $
_httpClientEE% 0
.EE0 1
	PostAsyncEE1 :
(EE: ;
urlEE; >
,EE> ?
contentEE@ G
)EEG H
;EEH I
_loggerGG 
.GG 
LogInformationGG &
(GG& '
$strGG' I
,GGI J
responseGGK S
.GGS T

StatusCodeGGT ^
)GG^ _
;GG_ `
returnII 
responseII 
;II  
}JJ 
catchKK 
(KK 
	ExceptionKK 
exKK 
)KK  
{LL 
_loggerMM 
.MM 
LogErrorMM  
(MM  !
exMM! #
,MM# $
$strMM% <
,MM< =
urlMM> A
)MMA B
;MMB C
throwNN 
newNN  
HttpRequestExceptionNN .
(NN. /
$"NN/ 1
$strNN1 A
{NNA B
urlNNB E
}NNE F
"NNF G
,NNG H
exNNI K
)NNK L
;NNL M
}OO 
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
HttpResponseMessageRR -
>RR- .
	PostAsyncRR/ 8
<RR8 9
TRR9 :
>RR: ;
(RR; <
stringRR< B
urlRRC F
,RRF G
TRRH I
payloadRRJ Q
)RRQ R
{SS 	
tryTT 
{UU 
varVV 
jsonVV 
=VV 
JsonSerializerVV )
.VV) *
	SerializeVV* 3
(VV3 4
payloadVV4 ;
)VV; <
;VV< =
varWW 
contentWW 
=WW 
newWW !
StringContentWW" /
(WW/ 0
jsonWW0 4
,WW4 5
EncodingWW6 >
.WW> ?
UTF8WW? C
,WWC D
$strWWE W
)WWW X
;WWX Y
_loggerYY 
.YY 
LogInformationYY &
(YY& '
$strYY' >
,YY> ?
urlYY@ C
)YYC D
;YYD E
var[[ 
response[[ 
=[[ 
await[[ $
_httpClient[[% 0
.[[0 1
	PostAsync[[1 :
([[: ;
url[[; >
,[[> ?
content[[@ G
)[[G H
;[[H I
_logger]] 
.]] 
LogInformation]] &
(]]& '
$str]]' I
,]]I J
response]]K S
.]]S T

StatusCode]]T ^
)]]^ _
;]]_ `
return__ 
response__ 
;__  
}`` 
catchaa 
(aa 
	Exceptionaa 
exaa 
)aa  
{bb 
_loggercc 
.cc 
LogErrorcc  
(cc  !
excc! #
,cc# $
$strcc% <
,cc< =
urlcc> A
)ccA B
;ccB C
throwdd 
newdd  
HttpRequestExceptiondd .
(dd. /
$"dd/ 1
$strdd1 A
{ddA B
urlddB E
}ddE F
"ddF G
,ddG H
exddI K
)ddK L
;ddL M
}ee 
}ff 	
}gg 
}hh À'
VC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\Services\EmailJobService.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
Services '
{ 
public 

class 
EmailJobService  
:! "
IEmailJobService# 3
{ 
private 
readonly 
ILogger  
<  !
EmailJobService! 0
>0 1
_logger2 9
;9 :
private 
readonly 
IHttpClientService +
_httpClientService, >
;> ?
private		 
readonly		 
IConfiguration		 '
_configuration		( 6
;		6 7
public 
EmailJobService 
( 
ILogger &
<& '
EmailJobService' 6
>6 7
logger8 >
,> ?
IHttpClientService@ R
httpClientServiceS d
,d e
IConfigurationf t
configuration	u Ç
)
Ç É
{ 	
_logger 
= 
logger 
; 
_httpClientService 
=  
httpClientService! 2
;2 3
_configuration 
= 
configuration *
;* +
} 	
public 
async 
Task 
SendEmailAsync (
(( )
string) /
correlationId0 =
,= >
string? E
toEmailF M
,M N
stringO U
subjectV ]
,] ^
string_ e
messagef m
,m n
into r

customerIds }
)} ~
{ 	
try 
{ 
_logger 
. 
LogInformation &
(& '
$str' q
,q r
correlationId	s Ä
,
Ä Å
toEmail
Ç â
)
â ä
;
ä ã
var 
emailPayload  
=! "
new# &
{ 
CorrelationId !
=" #
correlationId$ 1
,1 2
ToEmail 
= 
toEmail %
,% &
Subject 
= 
subject %
,% &
Message 
= 
message %
,% &

CustomerId 
=  

customerId! +
} 
; 
var!! 
emailApiUrl!! 
=!!  !
_configuration!!" 0
[!!0 1
$str!!1 L
]!!L M
??"" 
throw"" 
new""  %
InvalidOperationException""! :
("": ;
$str""; p
)""p q
;""q r
_logger$$ 
.$$ 
LogInformation$$ &
($$& '
$str$$' L
,$$L M
emailApiUrl$$N Y
)$$Y Z
;$$Z [
var&& 
response&& 
=&& 
await&& $
_httpClientService&&% 7
.&&7 8
	PostAsync&&8 A
(&&A B
emailApiUrl&&B M
,&&M N
emailPayload&&O [
)&&[ \
;&&\ ]
if(( 
((( 
response(( 
.(( 
IsSuccessStatusCode(( 0
)((0 1
{)) 
var** 
responseContent** '
=**( )
await*** /
response**0 8
.**8 9
Content**9 @
.**@ A
ReadAsStringAsync**A R
(**R S
)**S T
;**T U
_logger++ 
.++ 
LogInformation++ *
(++* +
$str+++ }
,++} ~
correlationId,, %
,,,% &
responseContent,,' 6
),,6 7
;,,7 8
}-- 
else.. 
{// 
var00 
errorContent00 $
=00% &
await00' ,
response00- 5
.005 6
Content006 =
.00= >
ReadAsStringAsync00> O
(00O P
)00P Q
;00Q R
_logger11 
.11 
LogError11 $
(11$ %
$str11% 
,	11 Ä
response22  
.22  !

StatusCode22! +
,22+ ,
errorContent22- 9
,229 :
correlationId22; H
)22H I
;22I J
throw33 
new33  
HttpRequestException33 2
(332 3
$"333 5
$str335 H
{33H I
response33I Q
.33Q R

StatusCode33R \
}33\ ]
$str33] _
{33_ `
errorContent33` l
}33l m
"33m n
)33n o
;33o p
}44 
}55 
catch66 
(66 
	Exception66 
ex66 
)66  
{77 
_logger88 
.88 
LogError88  
(88  !
ex88! #
,88# $
$str88% [
,88[ \
correlationId88] j
)88j k
;88k l
throw99 
;99 
}:: 
};; 	
}<< 
}== é
PC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\DTOs\PdfRequestDto.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
DTOs #
{ 
public 

class 
LogRequestDto 
{ 
public 
string 
CorrelationId #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
public 
string 
Service 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
public		 
string		 
Endpoint		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
=		- .
string		/ 5
.		5 6
Empty		6 ;
;		; <
[ 	
JsonRequired	 
] 
public 
DateTime 
	Timestamp !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
Payload 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
[ 	
JsonRequired	 
] 
public 
bool 
Success 
{ 
get !
;! "
set# &
;& '
}( )
} 
public 

class 
PdfRequestDto 
{ 
[ 	
JsonRequired	 
] 
public 
int 

CustomerId 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
JsonRequired	 
] 
public 
DateTime 
	StartDate !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
JsonRequired	 
] 
public 
DateTime 
EndDate 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
CorrelationId #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
[!! 	
JsonRequired!!	 
]!! 
public"" 
List"" 
<"" 
int"" 
>"" 
Products"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}"". /
=""0 1
new""2 5
List""6 :
<"": ;
int""; >
>""> ?
(""? @
)""@ A
;""A B
}## 
}$$ 	
SC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\DTOs\MessasingTaskDto.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
DTOs #
{ 
public 

class 
MessagingTaskDto !
{ 
public 
string 
CorrelationId #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public		 
string		 
Message		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
=		, -
string		. 4
.		4 5
Empty		5 :
;		: ;
[ 	
JsonRequired	 
] 
public 
int 

CustomerId 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} ”
OC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Flows\DTOs\EmailTaskDto.cs
	namespace 	
SERVERHANGFIRE
 
. 
Flows 
. 
DTOs #
{ 
public 

class 
EmailTaskDto 
{ 
public 
string 
CorrelationId #
{$ %
get& )
;) *
set+ .
;. /
}0 1
=2 3
string4 :
.: ;
Empty; @
;@ A
public 
string 
ToEmail 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
public 
string 
Subject 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
string. 4
.4 5
Empty5 :
;: ;
public		 
string		 
Message		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
=		, -
string		. 4
.		4 5
Empty		5 :
;		: ;
[

	 

JsonRequired


 
]

 
public 
int 

CustomerId 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} Ü]
UC:\Users\luisr\hangfire info\Proyectos-Info\HangFire\Controllers\ReportsController.cs
	namespace 	
SERVERHANGFIRE
 
. 
Controllers $
{ 
[		 
ApiController		 
]		 
[

 
Route

 

(


 
$str

 
)

 
]

 
public 

class 
ReportsController "
:# $
ControllerBase% 3
{ 
private 
readonly  
IBackgroundJobClient -
	_hangfire. 7
;7 8
private 
readonly 
ILogger  
<  !
ReportsController! 2
>2 3
_logger4 ;
;; <
public 
ReportsController  
(  ! 
IBackgroundJobClient  
hangfire! )
,) *
ILogger 
< 
ReportsController %
>% &
logger' -
)- .
{ 	
	_hangfire 
= 
hangfire  
;  !
_logger 
= 
logger 
; 
} 	
[ 	
HttpPost	 
] 
public 
IActionResult 
CreateReport (
(( )
[) *
FromBody* 2
]2 3
PdfRequestDto4 A
requestB I
)I J
{ 	
if 
( 
! "
ReportRequestValidator '
.' (
IsValid( /
(/ 0
request0 7
,7 8
out9 <
string= C
errorMessageD P
)P Q
)Q R
{ 
return 

BadRequest !
(! "
new" %
{& '
Error( -
=. /
errorMessage0 <
}= >
)> ?
;? @
} 
string!! 
correlationId!!  
=!!! "
string!!# )
.!!) *
IsNullOrWhiteSpace!!* <
(!!< =
request!!= D
.!!D E
CorrelationId!!E R
)!!R S
?"" 
Guid"" 
."" 
NewGuid"" 
("" 
)""  
.""  !
ToString""! )
("") *
)""* +
:## 
request## 
.## 
CorrelationId## '
;##' (
try%% 
{&& 
	_hangfire'' 
.'' 
Schedule'' "
<''" #
IReportJobService''# 4
>''4 5
(''5 6
job(( 
=>(( 
job(( 
.((  
ProcessReportRequest(( 3
(((3 4
request)) 
.))  

CustomerId))  *
,))* +
request** 
.**  
	StartDate**  )
,**) *
request++ 
.++  
EndDate++  '
,++' (
correlationId,, %
,,,% &
request-- 
.--  
Products--  (
).. 
,.. 
TimeSpan// 
.// 
FromMinutes// (
(//( )
$num//) *
)//* +
)00 
;00 
_logger22 
.22 
LogInformation22 &
(22& '
$str22' Z
,22Z [
correlationId22\ i
)22i j
;22j k
return44 
Ok44 
(44 
new44 
{55 
CorrelationId66 !
=66" #
correlationId66$ 1
,661 2
Status77 
=77 
$str77 (
,77( )
ScheduledTime88 !
=88" #
DateTime88$ ,
.88, -
UtcNow88- 3
.883 4

AddMinutes884 >
(88> ?
$num88? @
)88@ A
,88A B
Message99 
=99 
$str99 P
}:: 
):: 
;:: 
};; 
catch<< 
(<< 
	Exception<< 
ex<< 
)<<  
{== 
_logger>> 
.>> 
LogError>>  
(>>  !
ex>>! #
,>># $
$str>>% \
,>>\ ]
correlationId>>^ k
)>>k l
;>>l m
return?? 

StatusCode?? !
(??! "
$num??" %
,??% &
new??' *
{??+ ,
Error??- 2
=??3 4
$str??5 Q
}??R S
)??S T
;??T U
}@@ 
}AA 	
[CC 	
HttpPostCC	 
(CC 
$strCC "
)CC" #
]CC# $
publicDD 
IActionResultDD 
ScheduleEmailTaskDD -
(DD- .
[DD. /
FromBodyDD/ 7
]DD7 8
EmailTaskDtoDD9 E
	emailTaskDDF O
)DDO P
{EE 	
tryFF 
{GG 
_loggerHH 
.HH 
LogInformationHH &
(HH& '
$strHH' t
,HHt u
	emailTaskHHv 
.	HH Ä
CorrelationId
HHÄ ç
)
HHç é
;
HHé è
varJJ 
jobIdJJ 
=JJ 
	_hangfireJJ %
.JJ% &
ScheduleJJ& .
<JJ. /
IEmailJobServiceJJ/ ?
>JJ? @
(JJ@ A
jobKK 
=>KK 
jobKK 
.KK 
SendEmailAsyncKK -
(KK- .
	emailTaskLL !
.LL! "
CorrelationIdLL" /
,LL/ 0
	emailTaskMM !
.MM! "
ToEmailMM" )
,MM) *
	emailTaskNN !
.NN! "
SubjectNN" )
,NN) *
	emailTaskOO !
.OO! "
MessageOO" )
,OO) *
	emailTaskPP !
.PP! "

CustomerIdPP" ,
)QQ 
,QQ 
TimeSpanRR 
.RR 
FromMinutesRR (
(RR( )
$numRR) *
)RR* +
)SS 
;SS 
_loggerUU 
.UU 
LogInformationUU &
(UU& '
$strUU' 
,	UU Ä
jobId
UUÅ Ü
,
UUÜ á
	emailTask
UUà ë
.
UUë í
CorrelationId
UUí ü
)
UUü †
;
UU† °
returnWW 
OkWW 
(WW 
newWW 
{XX 
JobIdYY 
=YY 
jobIdYY !
,YY! "
CorrelationIdZZ !
=ZZ" #
	emailTaskZZ$ -
.ZZ- .
CorrelationIdZZ. ;
,ZZ; <
Status[[ 
=[[ 
$str[[ (
,[[( )
ScheduledTime\\ !
=\\" #
DateTime\\$ ,
.\\, -
UtcNow\\- 3
.\\3 4

AddMinutes\\4 >
(\\> ?
$num\\? @
)\\@ A
,\\A B
Message]] 
=]] 
$str]] A
,]]A B
EmailTo^^ 
=^^ 
	emailTask^^ '
.^^' (
ToEmail^^( /
}__ 
)__ 
;__ 
}`` 
catchaa 
(aa 
	Exceptionaa 
exaa 
)aa  
{bb 
_loggercc 
.cc 
LogErrorcc  
(cc  !
excc! #
,cc# $
$strcc% h
,cch i
	emailTaskccj s
.ccs t
CorrelationId	cct Å
)
ccÅ Ç
;
ccÇ É
returndd 

StatusCodedd !
(dd! "
$numdd" %
,dd% &
newdd' *
{dd+ ,
Errordd- 2
=dd3 4
$strdd5 X
,ddX Y
DetailsddZ a
=ddb c
exddd f
.ddf g
Messageddg n
}ddo p
)ddp q
;ddq r
}ee 
}ff 	
[hh 
HttpPosthh 
(hh 
$strhh "
)hh" #
]hh# $
publicii 
IActionResultii !
ScheduleMessagingTaskii *
(ii* +
[ii+ ,
FromBodyii, 4
]ii4 5
MessagingTaskDtoii6 F
messagingTaskiiG T
)iiT U
{jj 
trykk 
{ll 
ifnn 

(nn 
stringnn 
.nn 
IsNullOrWhiteSpacenn %
(nn% &
messagingTasknn& 3
.nn3 4
CorrelationIdnn4 A
)nnA B
||nnC E
stringoo 
.oo 
IsNullOrWhiteSpaceoo %
(oo% &
messagingTaskoo& 3
.oo3 4
PhoneNumberoo4 ?
)oo? @
||ooA C
stringpp 
.pp 
IsNullOrWhiteSpacepp %
(pp% &
messagingTaskpp& 3
.pp3 4
Messagepp4 ;
)pp; <
)pp< =
{qq 	
returnrr 

BadRequestrr 
(rr 
newrr !
{rr" #
Errorrr$ )
=rr* +
$strrr, a
}rrb c
)rrc d
;rrd e
}ss 	
_loggeruu 
.uu 
LogInformationuu 
(uu 
$str	vv û
,
vvû ü
messagingTaskww 
.ww 
CorrelationIdww '
,ww' (
messagingTaskxx 
.xx 
PhoneNumberxx %
,xx% &
messagingTaskyy 
.yy 
Messageyy !
,yy! "
messagingTaskzz 
.zz 

CustomerIdzz $
)zz$ %
;zz% &
var|| 
jobId|| 
=|| 
	_hangfire|| 
.|| 
Schedule|| &
<||& ' 
IMessagingJobService||' ;
>||; <
(||< =
job}} 
=>}} 
job}} 
.}} 
SendMessageAsync}} '
(}}' (
messagingTask~~ 
.~~ 
CorrelationId~~ +
,~~+ ,
messagingTask 
. 
PhoneNumber )
,) *
$str
ÄÄ 
,
ÄÄ 
messagingTask
ÅÅ 
.
ÅÅ 
Message
ÅÅ %
)
ÇÇ 
,
ÇÇ 
TimeSpan
ÉÉ 
.
ÉÉ 
FromMinutes
ÉÉ  
(
ÉÉ  !
$num
ÉÉ! "
)
ÉÉ" #
)
ÑÑ 	
;
ÑÑ	 

_logger
ÜÜ 
.
ÜÜ 
LogInformation
ÜÜ 
(
ÜÜ 
$str
ÜÜ n
,
ÜÜn o
jobId
ÜÜp u
,
ÜÜu v
messagingTaskÜÜw Ñ
.ÜÜÑ Ö
CorrelationIdÜÜÖ í
)ÜÜí ì
;ÜÜì î
return
àà 
Ok
àà 
(
àà 
new
àà 
{
ââ 	
JobId
ää 
=
ää 
jobId
ää 
,
ää 
CorrelationId
ãã 
=
ãã 
messagingTask
ãã )
.
ãã) *
CorrelationId
ãã* 7
,
ãã7 8
Status
åå 
=
åå 
$str
åå  
,
åå  !
ScheduledTime
çç 
=
çç 
DateTime
çç $
.
çç$ %
UtcNow
çç% +
.
çç+ ,

AddMinutes
çç, 6
(
çç6 7
$num
çç7 8
)
çç8 9
,
çç9 :
Message
éé 
=
éé 
$str
éé =
,
éé= >
PhoneNumber
èè 
=
èè 
messagingTask
èè '
.
èè' (
PhoneNumber
èè( 3
}
êê 	
)
êê	 

;
êê
 
}
ëë 
catch
íí 	
(
íí
 
	Exception
íí 
ex
íí 
)
íí 
{
ìì 
_logger
îî 
.
îî 
LogError
îî 
(
îî 
ex
îî 
,
îî 
$str
îî d
,
îîd e
messagingTask
îîf s
?
îîs t
.
îît u
CorrelationIdîîu Ç
)îîÇ É
;îîÉ Ñ
return
ïï 

StatusCode
ïï 
(
ïï 
$num
ïï 
,
ïï 
new
ïï "
{
ïï# $
Error
ïï% *
=
ïï+ ,
$str
ïï- T
,
ïïT U
Details
ïïV ]
=
ïï^ _
ex
ïï` b
.
ïïb c
Message
ïïc j
}
ïïk l
)
ïïl m
;
ïïm n
}
ññ 
}óó 
}
òò 
}ôô 
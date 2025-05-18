### [개요]

Unity Dots를 공부하기 위한 프로젝트입니다.
FPS장르로 만들었으며 간단한 캐릭터 이동, 총알 발사, 기존 유니티 모노비헤이비어와 상호작용이 되는지?, UniRx와 같은 라이브러리가 Dots와 어울리는지?, Dots를 사용하면서 디버깅 방법 등
이 프로젝트는 Dots에 익숙해지기 위함입니다.

### [기술 스택]

Unity3D, Unity6.0, Dots 1.3, UniRx

### [작업 인원]

본인 - 1명

### [Dots]

DOTS 자체는 프리릴리스로 꽤 오래전부터 존재해왔다는 사실은 알고 있었습니다. 이번에 Unity 6가 공개되면서 Unity에서 DOTS를 본격적으로 서포트하고 있는 모습을 보고, 저도 본격적으로 공부해볼까 하는 생각이 들었습니다.

Unity 공식 유튜브 영상에서 URP의 렌더링 패스가 DOP(Data-Oriented Programming) 방식으로 구성되었다는 내용을 본 기억이 있습니다. Unity는 모바일 게임 개발에 강점을 가진 엔진이고, 그만큼 성능 최적화를 중요시하는데, 이는 MonoBehaviour가 CPU 친화적이지 않다는 점에서 비롯된 선택이라는 생각이 들었습니다.

누구나 그렇겠지만, 처음 Unity 6를 설치하고 낯선 DOTS 환경에서 작업을 시작했을 때는 약 2주간은 익숙하지 않아 어려움이 있었습니다. 그러나 마치 OOP를 처음 익힐 때처럼, DOTS의 기초와 본질을 이해하게 되자 "아, 이렇게 구성되어야 하고, 이렇게 동작하는구나. 이런 방식이어서 유리하구나."라는 깨달음을 얻게 되었습니다.

DOTS가 있어서 좋은 점도 있지만, 무엇보다도 MonoBehaviour의 장점과 DOTS를 함께 연계해서 사용할 수 있다는 점이 가장 마음에 들었습니다. 이 프로젝트를 진행하면서 얻은 것은 단순히 기능 구현 능력뿐만 아니라, DOTS를 활용해 Authoring, Component, System을 유연하게 구성하는 방법, 디버깅 방법 등 실무적인 활용법에 대한 감각을 익힌 점입니다.

예를 들어, 프로그래머에게 MonoBehaviour 기반으로 Player 움직임을 만들어보라고 한다면, 어떤 사람은 간단히 컨트롤러를 만들어 Update에서 포지션을 직접 수정할 것이고, 또 어떤 사람은 CharacterController를 기반으로 EnemyController, PlayerController를 나눈 후, IMoveState를 통해 이동 로직을 구현하고, 상태 머신을 만들어 Update에서 동작을 호출할 것입니다. 이처럼 접근 방식은 다양하지만, 결국 모두 MonoBehaviour 위에서 동작하는 구조입니다.

결론적으로, 이 프로젝트를 통해 제가 DOTS를 완벽히 마스터하거나 아주 깊이 있게 파고들었다고 보긴 어렵습니다. 다만, DOTS가 어떤 식으로 구성되어 있고, 어떻게 동작하며, 향후 실무에서 DOTS를 사용할 일이 생긴다면 바로 적용할 수 있을 정도의 이해를 쌓는 데 중점을 두었습니다.

실제로 DOTS는 생각보다 진입 장벽이 높은 편이었고, 짧은 시간 안에 개념을 파악하는 것이 쉽지 않았습니다. 그러나 기본 개념과 구조를 이해한 뒤에는 훨씬 명확하게 동작 원리를 파악할 수 있었고, 그 덕분에 기존의 MonoBehaviour 기반 개발 방식과 비교해가며 DOTS의 효율성과 장점을 체감할 수 있었습니다.

### [캐릭터]

캐릭터의 이동, 상태머신은 유니티의 튜토리얼을 통해 먼저 선행학습 후 소스코드를 저의 게임에 맞게 리팩토링했습니다.

Dots는 SubScene에서 생명주기가 돌아가며 Unity의 대부분 컴퍼넌트를 지원하지 않습니다.
제가 사용한 에셋은 Skinned Mesh Renderer를 사용하고 있었고 이 또한 SubScene에서는 렌더링 되지 않습니다.
그렇기에 사용한 방법이 SubScene에는 빈 오브젝트만 두고 World에 해당 프리팹을 생성해서 둘이 동기화시키면 좋을거같다.
라는 생각을 하였고 그런 방식으로 찾아 본 결과 많은 사람들이 Hybrid로 이미 사용을 하고 있다는것을 알았습니다.

그래서 어디까지 Hybrid로 사용해야하냐? 라는 의문점이 들었고 저는 애니메이터를 Dots, World에서 모두 실행시켜보았습니다.
Dots에서는 상태에 따라 애니메이터의 파라미터를 변경시켰으며, World에서는 PlayerPresenter를 생성하여 Model의 리액티브프로퍼티를 통하여 플레이어가 무기를 스왑할때의 애니메이션을 실행시켰습니다.
사용 후 내린 결론은 단일 객체의 일부 시스템은 모노비헤이비어에서 처리하는게 좋다고 느꼈습니다.

예를들어
Update에서 계속 if(isSwapPressed) 이런식으로 호출하는건 비효율적이라고 생각합니다.
제가 사용한 방식과 같이 Dots에서 isPressed처리 -> Model에서 UniRx를 통한 옵저버 패턴 이용 -> 모노비헤이비어에서 처리
퍼포먼스는 단일 객체이기때문에 상관없었고 모노비헤이비어에서 처리하기때문에 코드 가독성이나 컴포넌트 사용의 유연함을 증가시켰습니다.
하지만 예외 상황도 있습니다. 예를들어 스왑하는중간에는 발사 금지 -> 이 상황일때에는 PlayerSystem에서 Input을 받기 때문에 Dots의 Component에 직접적으로 접근하여 Flag를 만들어줘야하는 상황이여서
모두 Dots에서 처리하는게 좋을수도있습니다.

### [적 이동]

적 이동을 구현하기까지 총 3가지의 방식을 썻습니다.

1. NavMeshAgent
   HybridData를 이용하여 world에 있는 프리팹의 NavMeshAgent 컴퍼넌트에 직접 setdestination하는 방법입니다. 가장 쉽고 Unity다운 개발 방식이긴 하지만
   저는 상태머신으로 캐릭터 MoveState에서 SubScene의 MoveVector의 값으로 Dots의 캐릭터유틸리티를 이용한 Move&Rotate를 처리했습니다.
   사실 이 방법이 아니여도 NavMeshAgent에 setdestination하는 방법을 사용한다면 모든 처리를 world에 있는 프리팹에게 맡겨야합니다.
   원래 방식은 SubScene의 Player Position -> World의 Player Position이였지만 이 방식을 사용한다면 World의 Player Position -> SubScene의 Player Position으로 동기화를 해야합니다.
   NavMeshAgent는 Class기반으로 Hybrid에서는 처리가 가능하지만 BurstCompile을 사용하는 부분에서 참조가 어렵고 Struct를 대부분 사용하는 Dots와는 개발 방식이 맞지 않다고 생각했습니다.

2. NavMeshPath를 직접 도출
   직접 Dots 소스내에서 Player, Enemy 간의 NavMeshPath를 직접 구해 MoveVector를 구하고 CharacterComponent에 지정해주는 방식입니다.
   단순히 손쉽게 유니티 Navigation 라이브러리를 사용할 수 있고 단순히 MoveVector를 구해서 MoveState를 Update하는거이기때문에
   여러가지 상황에서 상태 전환이 쉬웠습니다. 하지만 직선 집중 현상(적이 한점으로 모이는 현상)이 발생했고 이를 보완하기 위한 처리가 필요했습니다.
   이를 처리하기 위해 Obstacle 컴퍼넌트를 사용하여 재탐색을 하게도 해보았고 직접 적끼리 충돌시 랜덤으로 왼쪽 오른쪽으로 돌아 플레이어에 접근하는 식으로 Avoid 시스템을 개발해보았지만 해결은 됐었는데 제 마음에 드는 성과는없었습니다.
   또한 NavMeshPath의 결과에서 corners 프로퍼티를 get하면 GC가 발생한다는것을 인지하였고 이로인해 프레임드랍이 꽤 심했습니다.

   구글에 찾아보니 Unity에서는 이 문제를 해결하기위해 NavMeshQuery를 만들었지만 어째서인지 Obsolete가 되었습니다.
   아마 Unity측에서도 이 문제를 알고 있을거같고 향후에 최적화된 서비스를 제공할거 같습니다.

4. A* Pathfinding
   바닥에 깔린 Ground 오브젝트를 기반으로 Grid를 생성 -> 플레이어, 에너미까지의 Path를 정해 MoveVector로 변환 방식으로 구현했습니다.
   작동은 위 2번과 동일하였다고 생각합니다. 아직 그럴싸한 Dots용 Navigation 시스템이 구축되지 않은 상태에서는 현재로서 이 방법으로 고도화하는게 제일 좋을거 같습니다.

### [디버깅]

Unity Dots는 Systembase가 아닌 이상 대부분 BurstCompile를 사용합니다.
class이 아닌 struct에서 ISystem을 상속받아 사용하죠.
그렇기 때문에 UnityEngine에서 제공하는 Debug 기능을 못씁니다. 에디터에서는 일부 쓸 수 있습니다. 하지만 Job이 돌아갈때에는 사용이 안돼죠. (콘솔에서 에러 경고 뜹니다.)
처음에는 이 오류때문에 JobSystem을 사용하는 부분에서는 디버깅이 많이 어려웠습니다.
저는 이 문제를 해결 할 방법을 찾다가 생각해낸게 그럼 Debug용 System을 만들면 되지 않을까? 입니다.
DebugSystem을 만들어 SystemBase를 상속받아 UnityEngine에서 제공하는 Debug를 호출할 수 있게 해두었고
호출해야하는 Entity에 DebugComponent를 추가시켜 DebugSystem이 감지하면 그 내용을 호출하고 DebugComponent를 삭제하는 방식으로 사용했습니다.
많이 번거롭긴 하였지만 class 타입을 못쓰는 job에서는 이 방식이 가장 효율적이라고 생각 했습니다.


### [테스트]

Dots 사용X는 빈 프로젝트에 에셋만 동일하게 하였습니다. 

총 500마리의 Enemy를 소환하였으며,
Dots 사용 X 버전에서는 NavMeshAgent에 setdestination만 적용하였고
Dots 사용 O 버전에서는 현재 이 프로젝트 모든 기능들이 돌아가게하였습니다.

#### 1. Dots 사용 X
![1](https://github.com/user-attachments/assets/e6ad6dad-e292-42ce-9bf4-5fe80d65196f)


#### 2. Dots 사용 O
![3](https://github.com/user-attachments/assets/8ade13da-b9eb-4599-a67d-e3b18d0cb406)


### [결론]

아직 World에 의존하는게 많았으며 프로파일링결과 SystemBase, ISystem등 Job System을 쓰지않는경우 메인쓰레드에서 작동됩니다. ( UnityEngine에서 제공되는 Debug사용가능 )
생성형 AI가 최신 Dots는 학습하지않아 틀린답만 알려줍니다. (제가 이용을 못했던거일수도있습니다.) 구글링을 해도 예전 프리릴리스버전의 소스코드가 많습니다. 그래서 Unity 공식 도큐먼트와 튜토리얼을 보면서 익혔고 프로젝트 완성기간이 예상보다 꽤 많이 흘렀습니다.
뱀서라이크나 디펜스 장르의 게임에 아주 잘 어울릴거 같습니다. ( 정확히는 Object 수가 많은 게임 )

[👉 영상 보기 (Google Drive)](https://drive.google.com/file/d/15t-qNoPYVByihcpjAVcG3qwwRKJPItw9/view?usp=sharing)

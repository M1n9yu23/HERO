![main](https://github.com/user-attachments/assets/ec31d87b-dd2c-4766-a92a-6e2062fb1d82)

# 📖 프로젝트 개요
- 프로젝트명: HERO
- 장르: 2D 액션 플랫포머
- 사용 엔진: Unity
- 개발 언어: C#
- 목적: Unity를 활용하여 플레이 가능한 플랫포머 게임을 개발
- 특징: Angvik에서 영감을 받아 다양한 적, 맵, 난이도를 제공하는 도전적인 게임 플레이 제공

- 유료 에셋 사용으로 스크립트 파일만 올립니다. 또한, 모든 스크립트(플레이어, 함정, 맵, 설정, 사운드 등)가 아닌 몬스터의 주요 스크립트 파일만 올렸습니다.

# 👨‍👩‍👦‍👦 팀원 구성
| 장대훈 | 손민규 | 임동현 | 김경록 |
| --- | --- | --- | --- |
| 개발 | 개발 (몬스터AI, 패턴 구현) | 기획 | 기획 |

# 🎮 주요 기능
- **캐릭터 전투 시스템**: 몬스터를 공격하거나 몬스터의 공격을 피하면서 스테이지를 클리어하는 방식
- **몬스터 AI**: 고유한 패턴과 난이도를 가진 적들을 배치하여 도전적인 게임 플레이 제공. 특정 범위 내 플레이어 감지 후 다양한 패턴으로 공격 (근접, 원거리, 자폭 등)
- **맵 구성**: 난이도별 스테이지 제공, 다양한 함정 요소 포함
- **하드 코어**: 플레이어가 죽으면 처음부터 다시 시작해야하는 도전적인 구조
- UI 및 UX : 직관적인 게임 UI, 옵션 및 설정 기능 제공

# 🏗️ 개발 과정
1. **아이디어 기획**
    - 팀원들이 각자 아이디어를 정리하고 논의 후 플랫포머 장르 확정
    - 기존 플랫포머 게임 연구 및 컨셉 결정
2. **설계 및 개발**
    - Unity 및 C#을 활용한 게임 기초 시스템 구현
    - 에셋 선정 (BGM, 캐릭터, 몬스터, 맵 등)
    - 몬스터 AI 및 다양한 패턴 구현
    - 함정 및 장애물 기획 및 적용
3. **협업 및 테스트**
    - GitHub을 활용한 버전 관리 및 코드 공유
    - 주기적인 회의를 통한 피드백 반영
    - 플레이 테스트 진행 및 버그 수정

# 🛠️ 사용 기술
- 게임 엔진: Unity
- 프로그래밍 언어: C#
- 버전 관리 및 협업: GitHub, Notion

# 🧑‍💻 구현한 몬스터 AI 및 코드 설명
![prototype](https://github.com/user-attachments/assets/e14b57f7-f893-45bb-bfdf-5d53aad08bdf)

`위 사진은 프로토타입에 작성된 최종본입니다. 이 외의 몬스터들과 함정들은 위 사진 속 몬스터들을 기반으로 제작되었습니다. 주요 동작은 동일하며, 애니메이터 및 애니메이션 클립과 같은 요소만 다릅니다. 따라서 이 문서에서는 공통된 동작을 구현하는 기반 몬스터들에 대한 설명을 중심으로 다룹니다.`

### 기반이 되는 주요 스크립트 파일 설명
- **DetectionZone.cs** - 적 감지 시스템 관련 코드 (플레이어를 감지하고 특정 동작을 수행)
- **AnimationStrings.cs** - 애니메이션 관련 문자열 관리 코드 (애니메이터 상태 전환에 활용)
- **EnemyBase.cs** - 적 AI의 기본 클래스 (모든 적 캐릭터의 공통 로직 포함)
- **EnemyBaseMove.cs** - 적의 이동 관련 코드 (적별 이동 방식 및 AI 적용)
- **SetBoolBehaviour.cs** - 애니메이션 관련 Bool 값 조작 코드 (상태 머신에서 애니메이션 전환 제어)
- **SnakeBall.cs** - 투사체 관련 코드 (Snake 몬스터가 발사하는 오브젝트 및 타겟 추적 기능 포함)
- **TouchingDirections.cs** - 몬스터의 충돌 감지 및 방향 관련 코드 (벽, 바닥, 천장 감지)

### 몬스터 AI 개요
몬스터는 다양한 패턴과 동작을 수행하며, 플레이어와의 상호작용을 기반으로 동작합니다. 각각의 몬스터는 특정 조건을 만족하면 공격하거나 이동하며, 다양한 인공지능 기법이 적용되었습니다. ***또한 Detection Zone을 활용하여 특정 범위 내에 플레이어가 접근하면 함정이 작동하도록 구현했습니다. 예를 들어, Snake 몬스터는 일정 범위 내에 플레이어가 들어오면 투사체를 발사하도록 설정했습니다. 이처럼 함정도 이 와 같은 메커니즘으로 작동합니다.*** 

- AI 및 게임 메커니즘:
    - Unity Trigger 시스템을 활용한 몬스터 AI 구현
    - 오브젝트 풀링(Object Pooling)을 이용한 투사체 관리
        - *Unity의 Destroy()를 반복적으로 호출하면 메모리 할당과 해제가 반복되면서 성능 저하가 발생합니다. 이를 방지하기 위해 오브젝트 풀링을 활용하여 일정 수의 투사체를 미리 생성하고, 비활성화 및 재활용하는 방식으로 성능을 최적화했습니다. 스레드풀이 필요한 이유와 비슷합니다.*
    - Raycast를 활용한 환경 감지 및 상호작용
    - Animator와 State Machine을 활용한 애니메이션 시스템
    - Rigidbody2D를 이용한 물리 기반 몬스터 이동
    - Detection Zone을 사용한 적 감지
    - Waypoint를 활용한 몬스터 이동 패턴 설정

### 주요 구현 요소
1. **적 유형 (Enemy Type) 분류**
    - FlyEnemy (비행 몬스터)
    - Knight (근접 공격 몬스터)
    - Snake (원거리 공격 몬스터, 투사체 발사)
    - VultureEnemy (특정 경로(WayPoint)를 따라 이동하는 몬스터)
2. **공격 시스템**
    - DetectionZone을 활용한 플레이어 감지
    - Raycast를 활용한 환경 감지 및 충돌 처리 (장애물 감지 후 이동 방향 변경)
    - State Machine을 활용한 몬스터 상태 관리 (Idle, Move, Attack 등)
3. **이동 시스템**
    - Waypoint (VultureEnemy의 노란색 오브젝트)를 활용한 몬스터 이동 패턴 적용
    - Rigidbody2D 및 Physics를 활용한 자연스러운 움직임
    - FlipDirection() 함수를 활용한 이동 방향 전환
4. **오브젝트 풀링을 활용한 투사체 관리**
    - Snake 몬스터는 일정 시간마다 투사체(SnakeBall)를 발사
    - 오브젝트 풀링을 활용하여 메모리 효율적인 투사체 관리
    - 타겟을 추적하는 유도 기능 구현
5. 애니메이션 시스템
    - `AnimationStrings.cs`: 애니메이션 상태 전환을 위한 키 값 관리 (코드에서 직접 문자열 사용 방지)
    - `SetBoolBehaviour.cs`: 특정 애니메이션 상태에서 Boolean 값을 변경하여 상태 전환 (상태 머신 패턴)
6. 물리 및 충돌 시스템
    - `TouchingDirections.cs`: 바닥 및 벽 감지를 통한 충돌 처리
    - `Raycast`를 활용한 지형 감지 및 AI 행동 제어

# 📷 Screenshot
![play1](https://github.com/user-attachments/assets/35b7d868-762f-48be-9299-ddbd2843a53b)

![play2](https://github.com/user-attachments/assets/77d0ccae-c466-43e1-9bce-f5af33f29d2d)

![paly3](https://github.com/user-attachments/assets/e425a26f-2bc3-4267-9e33-7db3c63a9cfe)

![play4](https://github.com/user-attachments/assets/869eefd6-5894-4839-a58c-6815dcd62538)

![play5](https://github.com/user-attachments/assets/289f97e9-a6b9-430a-b9b6-5981dce8b611)

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 해당 컴포넌트를 가지고 있는 게임 오브젝트를 찾아서 반환한다.
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null) // 인스턴스를 찾지 못한 경우
                {
                    // 새로운 게임 오브젝트를 생성하여 해당 컴포넌트를 추가한다.
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    // 생성된 게임 오브젝트에서 해당 컴포넌트를 instance에 저장한다.
                    instance = obj.GetComponent<T>();

                }

                DontDestroyOnLoad(instance.gameObject); // 인스턴스의 게임 오브젝트를 DontDestroyOnLoad로 설정
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // 인스턴스의 게임 오브젝트를 파괴
        }
    }

    // 매니저를 제거하고 싶은 경우 호출
    public void DestroyManager()
    {
        if (instance == this)
        {
            instance = null;
            Destroy(gameObject); // 인스턴스의 게임 오브젝트를 파괴
        }
    }
}

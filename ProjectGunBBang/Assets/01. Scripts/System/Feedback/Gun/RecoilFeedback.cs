using UnityEngine;

namespace GB.Feedbacks
{
    public class RecoilFeedback : Feedback
    {
        private GameObject cameraRot;

        // Rotation
        private Vector3 _currentRecoilRotation;
        private Vector3 _targetRecoilRotation;

        // Hipfire Recoil
        [SerializeField] private float _recoilX;
        [SerializeField] private float _recoilY;
        [SerializeField] private float _recoilZ;

        // Settings
        [SerializeField] private float _snappiness;
        [SerializeField] private float _returnSpeed;

        private void Start()
        {
            cameraRot = GameObject.FindWithTag("Head");
        }

        private void Update()
        {
            // Recoil 회전 값 업데이트
            _targetRecoilRotation = Vector3.Lerp(_targetRecoilRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
            _currentRecoilRotation = Vector3.Slerp(_currentRecoilRotation, _targetRecoilRotation, _snappiness * Time.deltaTime);

            // 기존의 카메라 회전에 Recoil 회전을 더함
            cameraRot.transform.localRotation = Quaternion.Euler(cameraRot.transform.localRotation.eulerAngles + _currentRecoilRotation);
        }

        protected override void OnPlay(Vector3 playPos)
        {
            RecoilFire();
        }

        public void RecoilFire()
        {
            _targetRecoilRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
        }
    }
}

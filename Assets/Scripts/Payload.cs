using UnityEngine;

public class Payload : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private Texture2D[] _emissionTextures = new Texture2D[0];
    [SerializeField] private float _minVelocity = -1.0f;
    [SerializeField] private float _maxVelocity = 1.0f;
    [SerializeField] private float _minScale = 0.25f;
    [SerializeField] private float _maxScale = 2f;
    [SerializeField] private float _minEmissionColorIntensity = 1f;
    [SerializeField] private float _maxEmissionColorIntensity = 2f;
    [SerializeField] private float _minColorChangeTime = 1f;
    [SerializeField] private float _maxColorChangeTime = 6f;
    [SerializeField] private float _minColorIntensityChangeTime = 1f;
    [SerializeField] private float _maxColorIntensityChangeTime = 3f;

    private float _nextColorChangeTime = 0f;
    private float _nextColorIntensityChangeTime = 0f;
    private int _nextColorIndex = 0;
    private float _nextColorIntensity;

    [SerializeField] private Color[] _colors = new Color[1];

    private Color _currentColor = new Color();
    private float _currentColorIntensity = 1;

    private Material _baseMaterial = null;

    private Material _usedMaterial;
    private Rigidbody _rigidBody;
    private int _emissionColorId;
    private int _emissionTextureId;
    private bool _initDone;
    #endregion

    #region Unity Functions
    private void CustomAwake()
    {
        _emissionColorId = Shader.PropertyToID("_EmissionColor");
        _emissionTextureId = Shader.PropertyToID("_EmissionMap");

        float scale = Random.Range(_minScale, _maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.velocity = new Vector3(Random.Range(_minVelocity, _maxVelocity), Random.Range(_minVelocity, _maxVelocity), Random.Range(_minVelocity, _maxVelocity));
        _usedMaterial = new Material(_baseMaterial);
        GetComponent<Renderer>().material = _usedMaterial;
        _nextColorIndex = Random.Range(0, _colors.Length);
        _currentColor = _colors[_nextColorIndex];
        _currentColorIntensity = Random.Range(_minEmissionColorIntensity, _maxEmissionColorIntensity);
        _usedMaterial.SetColor(_emissionColorId, _currentColor * _currentColorIntensity);
        _usedMaterial.SetTexture(_emissionTextureId, _emissionTextures[Random.Range(0, _emissionTextures.Length - 1)]);
        _usedMaterial.SetTexture("_EmissiveColorMap", _emissionTextures[Random.Range(0, _emissionTextures.Length - 1)]);
        _initDone = true;
    }

    private void Update()
    {
		if (_initDone)
		{
            if (Time.time > _nextColorChangeTime)
            {
                _nextColorChangeTime += Random.Range(_minColorChangeTime, _maxColorChangeTime);
                _nextColorIndex++;
                if (_nextColorIndex >= _colors.Length)
                    _nextColorIndex = 0;
            }
            if (Time.time > _nextColorIntensityChangeTime)
            {
                _nextColorIntensityChangeTime += Random.Range(_minColorIntensityChangeTime, _maxColorIntensityChangeTime);
                _nextColorIntensity = Random.Range(_minEmissionColorIntensity, _maxEmissionColorIntensity);
            }

            _currentColor = Color.Lerp(_currentColor, _colors[_nextColorIndex], Time.smoothDeltaTime);
            _currentColorIntensity = Mathf.Lerp(_currentColorIntensity, _nextColorIntensity, Time.smoothDeltaTime);
            _usedMaterial.SetColor(_emissionColorId, _currentColor * _currentColorIntensity);
            _usedMaterial.SetColor("_EmissiveColor", _currentColor * _currentColorIntensity);
		}
    }

    private void OnCollisionEnter(Collision collision)
    {
		if (_initDone)
		{
            ContactPoint contact = collision.contacts[0];
            Vector3 reflectedVelocity = Vector3.Reflect(_rigidBody.velocity, contact.normal).normalized;
            _rigidBody.velocity = reflectedVelocity;
		}
    }
    #endregion

    #region Public Functions
    public void Initialize(PayloadScriptableObject data)
    {
        _emissionTextures[0] = (data.emissionTextures.Length > 1) ? data.emissionTextures[Random.Range(0, data.emissionTextures.Length - 1)] : data.emissionTextures[0];
        _colors = data.colors.colors;
        _baseMaterial = data.baseMaterial;
        CustomAwake();
    }
    #endregion
}

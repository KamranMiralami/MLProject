Install python python-3.9.13-amd64
python -m venv venv 
venv\scripts\activate
python -m pip install --upgrade pip
pip install mlagents
pip install torch torchvision torchaudio
pip install protobuf==3.20.3
mlagents-learn --help
pip install onnx
mlagents-learn config_name.yaml --run-id=run_name
tensorboard --logdir results

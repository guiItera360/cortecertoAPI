# tests/test_face_shape.py
from model.face_shape_detector import FaceShapeDetector

def test_face_shape_classification():
    detector = FaceShapeDetector()
    landmarks, _ = detector.detect_landmarks("assets/imagem_teste.png")
    shape = detector.classify_face_shape(landmarks)
    assert shape in ["Oval", "Redondo", "Quadrado", "Coração", "Diamante", "Oblongo"]

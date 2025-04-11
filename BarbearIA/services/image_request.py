import uuid, os, shutil
from fastapi import UploadFile
from model.face_shape_detector import FaceShapeDetector
from model.haircut_recommender import recommend_haircut

TEMP_DIR = "temp_images"
os.makedirs(TEMP_DIR, exist_ok=True)

async def process_image_request(file: UploadFile, nome_cliente: str):
    filename = f"{uuid.uuid4().hex}_{file.filename}"
    image_path = os.path.join(TEMP_DIR, filename)

    with open(image_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)

    try:
        detector = FaceShapeDetector()
        landmarks, _ = detector.detect_landmarks(image_path)
        face_shape = detector.classify_face_shape(landmarks)
        recomendacoes = recommend_haircut(face_shape)

        return {
            "cliente": nome_cliente,
            "formato_rosto": face_shape,
            "recomendacoes_corte": recomendacoes
        }

    finally:
        if os.path.exists(image_path):
            os.remove(image_path)

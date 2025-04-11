# model/face_shape_detector.py

import cv2
import mediapipe as mp
import numpy as np

class FaceShapeDetector:
    def __init__(self):
        # Inicializa os utilitários do MediaPipe para malha facial
        self.mp_face_mesh = mp.solutions.face_mesh
        self.face_mesh = self.mp_face_mesh.FaceMesh(static_image_mode=True)
        self.drawing_utils = mp.solutions.drawing_utils

    def detect_landmarks(self, image_path):
        """
        Carrega uma imagem, detecta landmarks faciais e retorna as coordenadas dos pontos do rosto.

        Parâmetro:
        - image_path (str): Caminho para a imagem de entrada.

        Retorno:
        - landmarks (list): Lista de tuplas (x, y) com as coordenadas dos pontos.
        - image (numpy.ndarray): Imagem original carregada.
        """
        image = cv2.imread(image_path)
        if image is None:
            raise ValueError(f"Imagem não encontrada no caminho: {image_path}")

        # Converte para RGB, exigência do MediaPipe
        rgb_image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = self.face_mesh.process(rgb_image)

        if not results.multi_face_landmarks:
            raise ValueError("Nenhum rosto detectado na imagem.")

        # Assume o primeiro rosto detectado
        face_landmarks = results.multi_face_landmarks[0]

        # Converte pontos normalizados em coordenadas reais da imagem
        height, width, _ = image.shape
        landmarks = [
            (int(point.x * width), int(point.y * height))
            for point in face_landmarks.landmark
        ]

        return landmarks, image

    def classify_face_shape(self, landmarks):
        """
        Classifica o formato do rosto com base em proporções entre landmarks.

        Parâmetro:
        - landmarks (list): Lista de tuplas (x, y) com coordenadas faciais.

        Retorno:
        - face_shape (str): String em PT-BR representando o formato do rosto identificado.
        """
        # Define landmarks estratégicas pelo índice do MediaPipe
        jaw_left = landmarks[234]    # Lado esquerdo do maxilar
        jaw_right = landmarks[454]   # Lado direito do maxilar
        chin = landmarks[152]        # Queixo
        forehead = landmarks[10]     # Topo da testa
        cheek_left = landmarks[93]   # Bochecha esquerda
        cheek_right = landmarks[323] # Bochecha direita

        # Distância Euclidiana entre dois pontos
        def distance(p1, p2):
            return np.linalg.norm(np.array(p1) - np.array(p2))

        jaw_width = distance(jaw_left, jaw_right)
        face_height = distance(forehead, chin)
        cheekbone_width = distance(cheek_left, cheek_right)

        aspect_ratio = face_height / jaw_width

        # Regras de classificação simples com retorno em PT-BR
        if aspect_ratio > 1.6:
            return "Oblongo"
        elif abs(jaw_width - cheekbone_width) < 20 and aspect_ratio < 1.3:
            return "Redondo"
        elif abs(jaw_width - cheekbone_width) < 20 and aspect_ratio >= 1.3:
            return "Quadrado"
        elif cheekbone_width > jaw_width:
            return "Coração"
        elif cheekbone_width < jaw_width:
            return "Diamante"
        else:
            return "Oval"

# model/haircut_recommender.py

def recommend_haircut(face_shape: str) -> list:
    recommendations = {
        "Oval": [
            "Undercut com volume",
            "Pompadour",
            "Quiff clássico",
            "Corte Caesar moderno"
        ],
        "Redondo": [
            "Topete estruturado",
            "Corte com laterais baixas",
            "Faux Hawk",
            "Pompadour angular"
        ],
        "Quadrado": [
            "Corte com fade baixo",
            "Buzz cut",
            "Corte militar",
            "Pompadour com laterais curtas"
        ],
        "Coração": [
            "Franja lateral",
            "Corte médio bagunçado",
            "Side part",
            "Corte com volume na parte superior"
        ],
        "Diamante": [
            "Corte texturizado",
            "Messy fringe",
            "Side swept",
            "Corte médio com volume"
        ],
        "Oblongo": [
            "Corte médio equilibrado",
            "Topete baixo",
            "Side part com volume moderado"
        ],
        "Desconhecido": [
            "Rosto não reconhecido",
            "Tente outra imagem"
        ]
    }

    return recommendations.get(face_shape, ["Sem sugestões no momento."])

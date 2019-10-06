from rest_framework.response import Response
from rest_framework import status


def badRequestError(request, data=''):
    return Response(data, status = status.HTTP_400_BAD_REQUEST)


def conflictError(request, data=''):
    return Response(data, status = status.HTTP_409_CONFLICT)


def forbiddenError(request, data=''):
    return Response(data, status = status.HTTP_403_FORBIDDEN)


def notFoundError(request, data=''):
    return Response(data, status = status.HTTP_404_NOT_FOUND)


def serverError(request, data=''):
    return Response(data, status = status.HTTP_500_INTERNAL_SERVER_ERROR)


def unAuthorizedError(request, data=''):
    return Response(data, status = status.HTTP_401_UNAUTHORIZED)


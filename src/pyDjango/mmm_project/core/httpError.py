from rest_framework.response import Response


def badRequestError(request, data=''):
    return Response({
        'status': 400,
        'data': data
    })


def conflictError(request, data=''):
    return Response({
        'status': 409,
        'data': data
    })


def forbiddenError(request, data=''):
    return Response({
        'status': 403,
        'data': data
    })


def notFoundError(request, data=''):
    return Response({
        'status': 404,
        'data': data
    })


def serverError(request, data=''):
    return Response({
        'status': 500,
        'data': data
    })


def unAuthorizedError(request, data=''):
    return Response({
        'status': 401,
        'data': data
    })

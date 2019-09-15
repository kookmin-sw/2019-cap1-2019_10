from rest_framework.response import Response


def ok(request, data=''):
    return Response({
        'status': 200,
        'data': data
    })


def created(request, data=''):
    return Response({
        'status': 201,
        'data': data
    })


def noContent(request, data=''):
    return Response({
        'status': 204,
        'data': data
    })

from django.http.response import HttpResponse

class Unity3DMiddleware(object):
    """
    An Unity3D middleware which replaces the response status code with 200
    and adds a REAL_STATUS_CODE header containing the original status code.
    It also replaces the request method with the one specified in the 'UNITY_METHOD'
    header field.
    These modifications are necessary if you want to work with Unity's WWW class and
    still keep the original Django RestFramework conventions.
    """
    def __init__(self, get_response):
        self.get_response = get_response

    def __call__(self, request):
        return self.get_response(request)

    def process_exception(self, request, exception): 
        return HttpResponse("in exception")

    def process_request(self, request):
        """
        Replaces the request method with the method defined in the
        UNITY_METHOD header field(if specified).
        """
        if 'HTTP_X_UNITY_METHOD' in request.META:
            method = request.META['HTTP_X_UNITY_METHOD']
            if method in ['POST', 'DELETE', 'PUT', 'GET']:
                request.method = method

    def process_response(self, request, response):
        """
        Replaces the status code with 200 and adds a 'REAL_STATUS' header field with the
        original status code and text.
        """
        if 'application/json' in request.META.get('HTTP_ACCEPT'):
            response["REAL_STATUS"] = '%s %s' % (response.status_code, getattr(response, 'status_text', ''))
            response.status_code = 200
        return response

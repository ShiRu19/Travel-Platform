function ShowErrorMessage(error) {
    console.log('StatusCode: ' + error.response.status);
    console.log('Error: ' + error.response.data.error);
    console.log('Message: ' + error.response.data.message);
}
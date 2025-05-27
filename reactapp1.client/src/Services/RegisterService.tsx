import axios from 'axios';


export async function register(username: string, password: string) {
    try {
        const response = await axios.post('https://localhost:7089/api/Auth/Login', {
            username,
            password,
        });
        return response.data;
    } catch (error: unknown) {
        if (error instanceof Error) {
            console.error(error.message);
        } else {
            console.error('Unknown error', error);
        }
    }
}
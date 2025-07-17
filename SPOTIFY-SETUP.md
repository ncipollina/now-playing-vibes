# 🎵 Spotify Developer Account Setup Guide

Follow these steps to create your Spotify Developer account and configure the app for Now Playing Vibes.

## Step 1: Create Spotify Developer Account

1. **Go to Spotify Developer Dashboard**
   - Visit: https://developer.spotify.com/
   - Click "Get Started" or "Log in"

2. **Sign in with your Spotify account**
   - Use your existing Spotify account
   - If you don't have one, create a free Spotify account first

3. **Accept Developer Terms**
   - Read and accept the Spotify Developer Terms of Service
   - Complete any required developer registration steps

## Step 2: Create Your App

1. **Go to Dashboard**
   - Navigate to https://developer.spotify.com/dashboard/
   - Click "Create App"

2. **Fill out App Details**
   ```
   App Name: Now Playing Vibes
   App Description: A vibe-sharing app that plays music based on user's mood and shares music taste with team members
   Website: https://github.com/ncipollina/now-playing-vibes
   Redirect URI: http://127.0.0.1:3000/callback/spotify
   ```

3. **Select APIs**
   - ✅ Web API
   - ✅ Web Playback SDK

4. **Accept Terms and Create**
   - Read and accept the terms
   - Click "Create"

## Step 3: Configure App Settings

1. **Get Your Credentials**
   - Copy the **Client ID** (you'll need this)
   - Click "Show Client Secret" and copy it (keep this secure!)

2. **Set App Settings**
   - Click "Edit Settings"
   - Add these **Redirect URIs**:
     - `http://127.0.0.1:3000/callback/spotify`
     - `https://localhost:5287/callback/spotify` (for client-side auth)
   - Save changes

3. **Development Mode**
   - Your app starts in "Development Mode"
   - This allows up to 25 users to test the app
   - Perfect for team testing!

## Step 4: Add Test Users (Optional)

1. **Go to User Management**
   - In your app dashboard, click "Users and Access"
   - Click "Add New User"

2. **Add Your Co-workers**
   - Enter their Spotify email addresses
   - They'll receive invitations to test the app

## Step 5: Configure Local Development

1. **Create User Secrets (Recommended)**
   ```bash
   # In the VibeTracker.Server directory
   dotnet user-secrets set "Spotify:ClientId" "your-client-id-here"
   dotnet user-secrets set "Spotify:ClientSecret" "your-client-secret-here"
   ```

2. **Or Add to appsettings.Development.json**
   ```json
   {
     "Spotify": {
       "ClientId": "your-client-id-here",
       "ClientSecret": "your-client-secret-here",
       "RedirectUri": "http://127.0.0.1:3000/callback/spotify"
     }
   }
   ```

## Step 6: Test Your Setup

1. **Check App Status**
   - In Spotify Dashboard, verify your app is "Active"
   - Note the number of users (starts at 25 for development)

2. **Test Scopes**
   - We'll need these scopes for the app:
     - `user-read-private` (read user profile)
     - `user-read-email` (read user email)
     - `streaming` (play music in browser)
     - `user-modify-playback-state` (control playback)

## Important Notes

### 🔒 Security
- **Never commit your Client Secret** to version control
- Use user secrets or environment variables for credentials
- Keep your Client ID public-facing (it's safe to expose)

### 🎵 Spotify Premium
- Full playback requires Spotify Premium subscription
- Free users can only play 30-second previews
- We'll implement fallbacks for free users

### 📊 Rate Limits
- Spotify API has rate limits (100 requests per minute)
- We'll implement proper rate limiting in the code

### 🧪 Development Mode
- Development mode allows 25 test users
- Perfect for team testing before going live
- No approval needed for basic functionality

## Next Steps

Once you have your **Client ID** and **Client Secret**:

1. ✅ Complete this setup guide
2. ✅ Configure the credentials in the app
3. ✅ Test the OAuth flow
4. ✅ Implement music discovery features
5. ✅ Add playback functionality

## Need Help?

If you run into issues:
- Check the [Spotify Web API Documentation](https://developer.spotify.com/documentation/web-api/)
- Review the [Web Playback SDK Guide](https://developer.spotify.com/documentation/web-playback-sdk/)
- Common issues are usually related to redirect URIs or scopes

**Ready to rock! 🎸** Once you have your credentials, we can start implementing the Spotify integration!
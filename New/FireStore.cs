using Google.Cloud.Firestore;

public class FireStore {
    public static FirestoreDb GetClient(string projectId) {
        return FirestoreDb.Create(projectId);
    }
}
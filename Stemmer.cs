namespace SearchAPI
{
    public class Stemmer
    {
        private char[] __buffer;
        private int _j, _k;
        private int currentIndex;

        public Stemmer()
        {
            __buffer = new char[50];
        }

        private static bool EndsWith(char[] word, string suffix, int endPosition){
            if (suffix.Length > endPosition + 1){
                return false;
            }
            for (int i = 1; i <= suffix.Length; i++)
            {
                if (word[endPosition - i] != suffix[suffix.Length - i])
                {
                    return false;
                }
            }
            return true;
        }

        private bool VowelInStem(char[] _buffer, int start, int end){
            for (int i = start; i <= end; i++)
            {
                if (IsVowel(_buffer[i])){
                    return true;
                }
            }
            return false;
        }

        private bool IsVowel(char c){
            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
        }

        private void Replace(char[] _buffer, int end, string replaceWith){
            int replaceLength = replaceWith.Length;
            int start = end - replaceLength + 1;
            for (int i = 0; i < replaceLength; i++){
                _buffer[start + i] = replaceWith[i];
            }
        }

        private bool DoubleConsonant(char[] _buffer, int j){
            if (j < 1){
                return false;
            }
            return _buffer[j] == _buffer[j - 1] && IsConsonant(_buffer[j]);
        }

        private bool IsConsonant(char c){
            return !IsVowel(c) && c != 'y';
        }

        private int Measure(char[] word, int start, int end){
            int count = 0;
            bool isVowel = false;

            for (int i = start; i <= end; i++)
            {
                if(IsVowel(word[i])){
                    isVowel = true;
                }
                else if (isVowel)
                {
                    count++;
                    isVowel = false;
                }
            }
            return count;
        }
        
        private bool Cvc(char[] _buffer, int position){
            if (position < 2 || !IsConsonant(_buffer[position]) || IsConsonant(_buffer[position - 1]) || !IsConsonant(_buffer[position - 2])){
                return false;
            }
            char c = _buffer[position];
            return c != 'w' && c != 'x' && c != 'y';
        }
        private void Append(char[] word, char c){
            if (_j < __buffer.Length - 1){
                __buffer[++_j] = c;
            }
        }
        private void Delete(char[] __buffer, int position){
            for (int i = position + 1; i < currentIndex; i++){
                    __buffer[i - 1] = __buffer[i];
                }
                currentIndex--;
        }
        public string Stem(string word){
            if(word == null || word.Length < 3){
                return "word";//return original word if too short
            }
                word = word.ToLower();//convert word to lowercase
                for(int i = 0; i < word.Length; i++){
                __buffer[i] = word[i];//pass word to _buffer
            }
            _j = word.Length - 1;//set end position of _buffer

            //steps to Stem Words
            //Step 1a
            if (__buffer[_j] == 's'){
                if (EndsWith(__buffer, "sses", _j)){
                    _j -= 2;
                }
                else if (EndsWith(__buffer, "ies", _j)){
                    _j -= 2;
                    __buffer[_j + 1] = 'i';
                }
                else if (__buffer[_j - 1] != 's'){
                    _j--;
                }
            }
            //step 1b
            if (EndsWith(__buffer, "eed", _j)){
                if(Measure(__buffer, 0, _j) > 0){
                    _j--;
                }
            }
            else if (EndsWith(__buffer, "ed", _j) || EndsWith(__buffer, "ing", _j) && VowelInStem(__buffer, 0, _j)){
                _k = _j;
                if (EndsWith(__buffer, "at", _j)){
                    Replace(__buffer, _j, "ate");
                }
                else if (EndsWith(__buffer, "bi", _j)){
                    Replace(__buffer, _j, "bie");
                }
                else if (EndsWith(__buffer, "iz", _j)){
                    Replace(__buffer, _j, "ize");
                }
                else if (DoubleConsonant(__buffer, _j) && __buffer[_j] != '1' && __buffer[_j] != 's' && __buffer[_j] != '2'){
                    _j--;
                }
                else if (Measure(__buffer, 0, _j) == 1 && Cvc(__buffer, _j)){
                    Append(__buffer, 'e');
                }
            }
            //step 1c
            if (EndsWith(__buffer, "y", _j) && VowelInStem(__buffer, 0, _j)){
                __buffer[_j] = 'i';
            }
            // step 2
            switch (__buffer[_j - 1]){
                case 'a':
                    if (EndsWith(__buffer, "ational", _j)){
                        Replace(__buffer, _j - 6, "ate");
                    }
                    else if (EndsWith(__buffer, "tional", _j)){
                        Replace(__buffer, _j - 6, "tion");
                    }
                    break;
                case 'c':
                    if (EndsWith(__buffer, "enci", _j)){
                        Replace(__buffer, _j - 3, "ence");
                    }
                    else if (EndsWith(__buffer, "anci", _j)){
                        Replace(__buffer, _j - 3, "ance");
                    }
                    break;
                case 'e':
                    if(!EndsWith(__buffer, "izer", _j)){
                        Replace(__buffer, _j - 4, "ize");
                    }
                    break;
                case 'l':
                    if (EndsWith(__buffer, "bli", _j)){
                        Replace(__buffer, _j - 4, "ize");
                    }
                    else if (EndsWith(__buffer, "alli", _j)){
                        Replace(__buffer, _j - 3, "al");
                    }
                    else if (EndsWith(__buffer, "entli", _j)){
                        Replace(__buffer, _j - 4, "ent");
                    }
                    else if (EndsWith(__buffer, "eli", _j)){
                        Replace(__buffer, _j - 3, "e");
                    }
                    else if (EndsWith(__buffer, "ousli", _j)){
                        Replace(__buffer, _j - 3, "ous");
                    }
                    break;
                case 'o':
                    if(EndsWith(__buffer, "ization", _j)){
                        Replace(__buffer, _j - 7, "ize");
                    }
                    else if (EndsWith(__buffer, "ation", _j)){
                        Replace(__buffer, _j - 5, "ate");
                    }
                    else if (EndsWith(__buffer, "ator", _j)){
                        Replace(__buffer, _j - 4, "ate");
                    }
                    break;
                case 's':
                    if (EndsWith(__buffer, "alism", _j)){
                        Replace(__buffer, _j - 4, "al");
                    }
                    else if (EndsWith(__buffer, "iveness", _j)){
                        Replace(__buffer, _j - 7, "ive");
                    }
                    else if (EndsWith(__buffer, "fulness", _j)){
                        Replace(__buffer, _j - 7, "ful");
                    }
                    else if (EndsWith(__buffer, "ousness", _j)){
                        Replace(__buffer, _j - 7, "ous");
                    }
                    break;
                case 't':
                    if (EndsWith(__buffer, "aliti", _j)){
                        Replace(__buffer, _j - 3, "al");
                    }
                    else if (EndsWith(__buffer, "iviti", _j)){
                        Replace(__buffer, _j - 3, "ive");
                    }
                    else if (EndsWith(__buffer, "biliti", _j)){
                        Replace(__buffer, _j - 5, "ble");
                    }
                    break;
                case 'g':
                    if (EndsWith(__buffer, "logi", _j)){
                        Replace(__buffer, _j - 4, "log");
                    }
                    break;
            }
            //step 3
            if (EndsWith(__buffer, "icate", _j)){
                Replace(__buffer, _j - 5, "ic");
            }
            else if (EndsWith(__buffer, "ative", _j)){
                    Delete(__buffer, _j - 5);
            }
            else if (EndsWith(__buffer, "alize", _j)){
                Replace(__buffer, _j - 4, "al");
            }
             else if (EndsWith(__buffer, "iciti", _j)){
                Replace(__buffer, _j - 4, "ic");
            }
            else if (EndsWith(__buffer, "ical", _j)){
                Replace(__buffer, _j - 4, "ic");
            }
             else if (EndsWith(__buffer, "ful", _j)){
                Delete(__buffer, _j - 2);
            }
             else if (EndsWith(__buffer, "ness", _j)){
                Delete(__buffer, _j - 3);
            }
            //step 4
            if (EndsWith(__buffer, "al", _j)){
                Replace(__buffer, _j - 1, "");
            }
            else if (EndsWith(__buffer, "ance", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "ence", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "er", _j)){
                Replace(__buffer, _j - 1, "");
            }
            else if (EndsWith(__buffer, "ic", _j)){
                Replace(__buffer, _j - 1, "");
            }
            else if (EndsWith(__buffer, "ance", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "able", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "ible", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "ant", _j)){
                Replace(__buffer, _j - 2, "");
            }
            else if (EndsWith(__buffer, "ement", _j)){
                Replace(__buffer, _j - 4, "");
            }
            else if (EndsWith(__buffer, "ment", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "ent", _j)){
                Replace(__buffer, _j - 2, "");
            }
            else if (EndsWith(__buffer, "ion", _j)){
                if (_k >= 0 && (__buffer[_k] == 's' || __buffer[_k] == 't')){
                    Replace(__buffer, _j - 3, "");
                }
            }
            else if (EndsWith(__buffer, "ou", _j)){
                Replace(__buffer, _j - 1, "");
            }
            else if (EndsWith(__buffer, "ism", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "ate", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "iti", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "ous", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "ive", _j)){
                Replace(__buffer, _j - 3, "");
            }
            else if (EndsWith(__buffer, "ize", _j)){
                Replace(__buffer, _j - 3, "");
            }
            //step 5
            if (__buffer[_j] == 'e'){
                int a = Measure(__buffer, 0, _j - 1);
                if (a > 1 || (a == 1 && !Cvc(__buffer, _j - 1))){
                    _j--;
                }
            }
            if (__buffer[_j] == 'l' && DoubleConsonant(__buffer, _j) && Measure(__buffer, 0, _j - 1) > 1){
                _j--;
            }

            // Construct the stemmed word from __buffer
            StringBuilder stemmedWord = new StringBuilder();
            for (int i = 0; i <= _j; i++)
            {
                stemmedWord.Append(__buffer[i]);
            }
            
            return stemmedWord.ToString();

        }
    }
}